/*
 *
 * (c) Copyright Ascensio System Limited 2010-2020
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/


using System;
using System.Collections.Generic;
using System.Linq;
using ASC.Common.Data.Sql;
using ASC.Core;
using ASC.Core.Tenants;
using ASC.Projects.Core.DataInterfaces;
using ASC.Projects.Core.Domain;
using ASC.Projects.Core.Domain.Reports;

namespace ASC.Projects.Data.DAO
{
    class ReportDao : BaseDao, IReportDao
    {
        private readonly string[] templateColumns = { "id", "type", "name", "filter", "cron", "create_by", "create_on", "tenant_id", "auto" };
        private readonly string[] columns = { "id", "type", "name", "fileId", "create_by", "create_on", "tenant_id" };
        private readonly Converter<object[], ReportTemplate> converter;
        private readonly Converter<object[], ReportFile> fileConverter;

        public ReportDao(int tenantID)
            : base(tenantID)
        {
            converter = ToTemplate;
            fileConverter = ToReportFile;
        }


        public List<ReportTemplate> GetTemplates(Guid userId)
        {
            return Db.ExecuteList(Query(ReportTemplateTable)
                                    .Select(templateColumns)
                                    .Where("create_by", userId.ToString())
                                    .OrderBy("name", true))
                .ConvertAll(converter);
        }

        public List<ReportTemplate> GetAutoTemplates()
        {
            return Db.ExecuteList(new SqlQuery(ReportTemplateTable).Select(templateColumns)
                                                        .Where("auto", true)
                                                        .OrderBy("tenant_id", true))
                .ConvertAll(converter);
        }

        public ReportTemplate GetTemplate(int id)
        {
            return Db.ExecuteList(Query(ReportTemplateTable).Select(templateColumns).Where("id", id)).ConvertAll(converter).SingleOrDefault();
        }

        public ReportTemplate SaveTemplate(ReportTemplate template)
        {
            if (template == null) throw new ArgumentNullException("template");
            if (template.CreateOn == default(DateTime)) template.CreateOn = DateTime.Now;
            if (template.CreateBy.Equals(Guid.Empty)) template.CreateBy = CurrentUserID;

            var insert = new SqlInsert(ReportTemplateTable, true)
                .InColumns(templateColumns)
                .Values(
                    template.Id,
                    template.ReportType,
                    template.Name,
                    template.Filter.ToXml(),
                    template.Cron,
                    template.CreateBy.ToString(),
                    TenantUtil.DateTimeToUtc(template.CreateOn),
                    Tenant,
                    template.AutoGenerated)
                .Identity(0, 0, true);
            template.Id = Db.ExecuteScalar<int>(insert);
            return template;
        }

        public void DeleteTemplate(int id)
        {
            Db.ExecuteNonQuery(Delete(ReportTemplateTable).Where("id", id));
        }



        public ReportFile Save(ReportFile reportFile)
        {
            if (reportFile == null) throw new ArgumentNullException("reportFile");
            if (reportFile.CreateOn == default(DateTime)) reportFile.CreateOn = DateTime.Now;
            if (reportFile.CreateBy.Equals(Guid.Empty)) reportFile.CreateBy = CurrentUserID;

            var insert = new SqlInsert(ReportTable, true)
                .InColumns(columns)
                .Values(
                    reportFile.Id,
                    reportFile.ReportType,
                    reportFile.Name,
                    reportFile.FileId,
                    reportFile.CreateBy.ToString(),
                    TenantUtil.DateTimeToUtc(reportFile.CreateOn),
                    CoreContext.TenantManager.GetCurrentTenant().TenantId)
                .Identity(0, 0, true);
            reportFile.Id = Db.ExecuteScalar<int>(insert);
            return reportFile;
        }

        public IEnumerable<ReportFile> Get()
        {
            return Db.ExecuteList(Query(ReportTable)
                                    .Select(columns)
                                    .Where("create_by", SecurityContext.CurrentAccount.ID)
                                    .OrderBy("create_on", false))
                .ConvertAll(fileConverter);
        }

        public ReportFile GetByFileId(int id)
        {
            return Db.ExecuteList(Query(ReportTable)
                .Select(columns)
                .Where("create_by", SecurityContext.CurrentAccount.ID)
                .Where("fileId", id))
                .ConvertAll(fileConverter).SingleOrDefault();
        }

        public void Remove(ReportFile report)
        {
            Db.ExecuteNonQuery(Delete(ReportTable)
                .Where("create_by", SecurityContext.CurrentAccount.ID)
                .Where("id", report.Id));
        }

        private static ReportTemplate ToTemplate(IList<object> r)
        {
            var tenant = CoreContext.TenantManager.GetTenant(Convert.ToInt32(r[7]));
            var template = new ReportTemplate((ReportType)Convert.ToInt32(r[1]))
            {
                Id = Convert.ToInt32(r[0]),
                Name = (string)r[2],
                Filter = r[3] != null ? TaskFilter.FromXml((string)r[3]) : new TaskFilter(),
                Cron = (string)r[4],
                CreateBy = ToGuid(r[5]),
                CreateOn = TenantUtil.DateTimeFromUtc(tenant.TimeZone, Convert.ToDateTime(r[6])),
                Tenant = tenant.TenantId,
                AutoGenerated = Convert.ToBoolean(r[8]),
            };
            return template;
        }

        private static ReportFile ToReportFile(IList<object> r)
        {
            var template = new ReportFile
            {
                Id = Convert.ToInt32(r[0]),
                ReportType = (ReportType)Convert.ToInt32(r[1]),
                Name = (string)r[2],
                FileId = r[3],
                CreateBy = ToGuid(r[4]),
                CreateOn = TenantUtil.DateTimeFromUtc(CoreContext.TenantManager.GetCurrentTenant().TimeZone, Convert.ToDateTime(r[5]))
            };
            return template;
        }
    }
}
