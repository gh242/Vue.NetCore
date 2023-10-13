/*
 *所有關於Sys_WorkFlow類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*Sys_WorkFlowService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
*/
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using System.Linq;
using VOL.Core.Utilities;
using System.Linq.Expressions;
using VOL.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.System.IRepositories;
using System.Collections.Generic;
using VOL.Core.WorkFlow;
using System;
using VOL.System.Repositories;

namespace VOL.System.Services
{
    public partial class Sys_WorkFlowService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_WorkFlowRepository _repository;//訪問數據庫
        private readonly ISys_WorkFlowStepRepository _stepRepository;
        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowService(
            ISys_WorkFlowRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ISys_WorkFlowStepRepository stepRepository
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _stepRepository = stepRepository;
            //多租戶會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }

        WebResponseContent webResponse = new WebResponseContent();
        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            saveDataModel.MainData["Enable"] = 1;


            AddOnExecuting = (Sys_WorkFlow workFlow, object list) =>
            {
                workFlow.WorkFlow_Id = Guid.NewGuid();

                webResponse= WorkFlowContainer.Instance.AddTable(workFlow, list as List<Sys_WorkFlowStep>);
                if (!webResponse.Status)
                {
                    return webResponse;
                }
                return webResponse.OK();
            };

            AddOnExecuted = (Sys_WorkFlow workFlow, object list) =>
            {
                return webResponse.OK();
            };
            return base.Add(saveDataModel);
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {
            Sys_WorkFlow flow = null;
            UpdateOnExecuting = (Sys_WorkFlow workFlow, object addList, object updateList, List<object> delKeys) =>
            {
                flow = workFlow;
                if ((flow.AuditingEdit ?? 0) == 0)
                {
                    if (Sys_WorkFlowTableRepository.Instance.Exists(x=>x.WorkFlow_Id==flow.WorkFlow_Id&&(x.AuditStatus == (int)AuditStatus.審核中)))
                    {
                        return webResponse.Error("當前流程有審核中的數據，不能修改,可以修改,流程中的【審核中數據是否可以編輯】屬性");
                    }
                }

                //新增的明細
                List<Sys_WorkFlowStep> add = addList as List<Sys_WorkFlowStep>;
                var stepsClone = add.Serialize().DeserializeObject<List<Sys_WorkFlowStep>>();
                add.Clear();

                var steps = _stepRepository.FindAsIQueryable(x => x.WorkFlow_Id == workFlow.WorkFlow_Id)
                 .Select(s => new { s.WorkStepFlow_Id, s.StepId })
                 .ToList();
                //刪除的節點
                var delIds = steps.Where(x => !stepsClone.Any(c => c.StepId == x.StepId))
                 .Select(s => s.WorkStepFlow_Id).ToList();
                delKeys.AddRange(delIds.Select(s=>s as object));

                //新增的節點
                var newSteps = stepsClone.Where(x => !steps.Any(c => c.StepId == x.StepId))
                .ToList();
                add.AddRange(newSteps);

                List<Sys_WorkFlowStep> update = updateList as List<Sys_WorkFlowStep>;
                //修改的節點
                var updateSteps = stepsClone.Where(x => steps.Any(c => c.StepId == x.StepId))
                .ToList();
                update.AddRange(updateSteps);
                updateSteps.ForEach(x =>
                {
                    x.WorkStepFlow_Id = steps.Where(c => c.StepId == x.StepId).Select(s => s.WorkStepFlow_Id).FirstOrDefault();
                    foreach (var item in saveModel.DetailData)
                    {
                        if (item["StepId"].ToString() == x.StepId)
                        {
                            item["WorkFlow_Id"] = workFlow.WorkFlow_Id;
                            item["WorkStepFlow_Id"] = x.WorkStepFlow_Id;
                        }
                    }
                });
                return webResponse.OK();
            };


            webResponse= base.Update(saveModel);
            if (webResponse.Status)
            {
                flow= repository.FindAsIQueryable(x => x.WorkFlow_Id == flow.WorkFlow_Id).Include(x=>x.Sys_WorkFlowStep).FirstOrDefault();
                webResponse = WorkFlowContainer.Instance.AddTable(flow,flow.Sys_WorkFlowStep);
            }
            return webResponse;
        }

        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            
            webResponse= base.Del(keys, delList);
            if (webResponse.Status)
            {
                WorkFlowContainer.DelRange(keys.Select(s=>(Guid)s.GetGuid()).ToArray());
            }
            return webResponse;
        }
    }
}
