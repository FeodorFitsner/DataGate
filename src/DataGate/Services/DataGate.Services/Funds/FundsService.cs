﻿// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
// Service class for managing funds

// Created: 09/2019
// Author:  Philip Shishov

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
namespace DataGate.Services.Funds
{
    using System;
    using System.Linq;
    using System.Data;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Microsoft.Extensions.Configuration;

    using DataGate.Data;
    using DataGate.Utilities.Services;
    using DataGate.Services.Funds.Contracts;

    // _____________________________________________________________
    public class FundsService : IFundsService
    {
        private readonly string defaultDate = DateTime.Today.ToString("yyyyMMdd");
        private readonly IConfiguration configuration;
        private readonly Pharus_vFinale_Context context;

        // ________________________________________________________
        //
        // Constructor: initialize with DI IConfiguration
        // to retrieve appsettings.json connection string
        public FundsService(
            IConfiguration config,
            Pharus_vFinale_Context context)
        {
            this.configuration = config;
            this.context = context;
        }

        // ________________________________________________________
        //
        // Retrieve query table DB based entities
        // with table functions

        public List<string[]> GetAllFunds(DateTime? chosenDate)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from fn_all_fund('{this.defaultDate}')";
                }
                else
                {
                    command.CommandText = $"select * from fn_all_fund('{chosenDate?.ToString("yyyyMMdd")}')";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllActiveFunds()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from fn_active_fund('{this.defaultDate}')";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllActiveFunds(DateTime? chosenDate)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from fn_active_fund('{this.defaultDate}')";
                }
                else
                {
                    command.CommandText = $"select * from fn_active_fund('{chosenDate?.ToString("yyyyMMdd")}')";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllFundsWithSelectedViewAndDate(
                                                                    List<string> preSelectedColumns,
                                                                    List<string> selectedColumns,
                                                                    DateTime? chosenDate)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                // Prepare items for DB query with []

                preSelectedColumns.AddRange(selectedColumns);
                preSelectedColumns = preSelectedColumns.Select(c => String.Format("[{0}]", c)).ToList();

                if (chosenDate == null)
                {
                    command.CommandText = $"select {string.Join(", ", preSelectedColumns)} from fn_all_fund('{this.defaultDate}')";
                }
                else
                {
                    command.CommandText = $"select {string.Join(", ", preSelectedColumns)} from fn_all_fund('{chosenDate?.ToString("yyyyMMdd")}')";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllActiveFundsWithSelectedViewAndDate(
                                                                            List<string> preSelectedColumns,
                                                                            List<string> selectedColumns,
                                                                            DateTime? chosenDate)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                // Prepare items for DB query with []

                preSelectedColumns.AddRange(selectedColumns);
                preSelectedColumns = preSelectedColumns.Select(c => String.Format("[{0}]", c)).ToList();

                if (chosenDate == null)
                {
                    command.CommandText = $"select {string.Join(", ", preSelectedColumns)} from fn_active_fund('{this.defaultDate}')";
                }
                else
                {
                    command.CommandText = $"select {string.Join(", ", preSelectedColumns)} from fn_active_fund('{chosenDate?.ToString("yyyyMMdd")}')";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string> GetAllFundsNames()
        {
            return this.context.TbHistoryFund
                .Select(f => f.FOfficialFundName)
                .ToList();
        }

        public List<string[]> GetFundWithDateById(DateTime? chosenDate, int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from fn_all_fund('{this.defaultDate}') where [FUND ID PHARUS] = {id}";
                }
                else
                {
                    command.CommandText = $"select * from fn_all_fund('{chosenDate?.ToString("yyyyMMdd")}') " +
                        $"where [FUND ID PHARUS] = {id}";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetFund_SubFunds(DateTime? chosenDate, int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from ActiveSubFundsForSpecificFundAtDate('{this.defaultDate}', {id})";
                }
                else
                {
                    command.CommandText = $"select * from ActiveSubFundsForSpecificFundAtDate('{chosenDate?.ToString("yyyyMMdd")}', {id})";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetFund_SubFundsWithSelectedViewAndDate(
                                                                    List<string> preSelectedColumns,
                                                                    List<string> selectedColumns,
                                                                    DateTime? chosenDate,
                                                                    int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                // Prepare items for DB query with []

                preSelectedColumns.AddRange(selectedColumns);
                preSelectedColumns = preSelectedColumns.Select(c => String.Format("[{0}]", c)).ToList();

                if (chosenDate == null)
                {
                    command.CommandText = $"select {string.Join(", ", preSelectedColumns)} from ActiveSubFundsForSpecificFundAtDate('{this.defaultDate}', {id}')";
                }
                else
                {
                    command.CommandText = $"select {string.Join(", ", preSelectedColumns)} from ActiveSubFundsForSpecificFundAtDate('{chosenDate?.ToString("yyyyMMdd")}', {id})";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetFundTimeline(int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from dbo.fn_timeline_fund({id})";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetDistinctFundDocuments(DateTime? chosenDate, int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from [dbo].[fn_view_distinct_documents_fund]('{this.defaultDate}', {id})";
                }
                else
                {
                    command.CommandText = $"select * from [dbo].[fn_view_distinct_documents_fund]('{chosenDate?.ToString("yyyyMMdd")}', {id})";
                }


                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllFundDocuments(int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from [dbo].[fn_view_documents_fund]({id})";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetDistinctFundAgreements(DateTime? chosenDate, int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from [dbo].[fn_view_distinct_agreements_fund]('{this.defaultDate}', {id})";
                }
                else
                {
                    command.CommandText = $"select * from [dbo].[fn_view_distinct_agreements_fund]('{chosenDate?.ToString("yyyyMMdd")}', {id})";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllFundAgreements(DateTime? chosenDate, int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from [dbo].[fn_view_agreements_fund]('{this.defaultDate}', {id})";
                }
                else
                {
                    command.CommandText = $"select * from [dbo].[fn_view_agreements_fund]('{chosenDate?.ToString("yyyyMMdd")}', {id})";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> PrepareFund_SubFundsForPDFExtract(DateTime? chosenDate)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from fn_active_subfund_pdf('{chosenDate?.ToString("yyyyMMdd")}')";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        // ________________________________________________________
        //
        // Execute query table DB based stored procedure
        // with fixed parameters
        public void EditFund(
                                int fundId,
                                string initialDate,
                                int fStatusId,
                                string regNumber,
                                string fundName,
                                string leiCode,
                                string cssfCode,
                                string faCode,
                                string depCode,
                                string taCode,
                                int fLegalFormId,
                                int fLegalTypeId,
                                int fLegalVehicleId,
                                int fCompanyTypeId,
                                string tinNumber,
                                string comment,
                                string commentTitle)
        {
            string query = "EXEC sp_modify_fund " +
                "@f_id, @f_initialDate, @f_status, " +
                "@f_registrationNumber, @f_officialFundName, @f_shortFundName, " +
                "@f_leiCode, @f_cssfCode, @f_faCode, @f_depCode, @f_taCode, " +
                "@f_legalForm, @f_legalType, @f_legal_vehicle, @f_companyType, @f_tinNumber, " +
                "@comment, @commentTitle";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@f_id", SqlDbType.Int) { Value = fundId },
                        new SqlParameter("@f_initialDate", SqlDbType.NVarChar) { Value = initialDate },
                        new SqlParameter("@f_status", SqlDbType.Int) { Value = fStatusId },
                        new SqlParameter("@f_registrationNumber", SqlDbType.NVarChar) { Value = regNumber },
                        new SqlParameter("@f_officialFundName", SqlDbType.NVarChar) { Value = fundName },
                        new SqlParameter("@f_shortFundName", SqlDbType.NVarChar) { Value = fundName },
                        new SqlParameter("@f_leiCode", SqlDbType.NVarChar) { Value = leiCode },
                        new SqlParameter("@f_cssfCode", SqlDbType.NVarChar) { Value = cssfCode },
                        new SqlParameter("@f_faCode", SqlDbType.NVarChar) { Value = faCode },
                        new SqlParameter("@f_depCode", SqlDbType.NVarChar) { Value = depCode },
                        new SqlParameter("@f_taCode", SqlDbType.NVarChar) { Value = taCode },
                        new SqlParameter("@f_legalForm", SqlDbType.Int) { Value = fLegalFormId },
                        new SqlParameter("@f_legalType", SqlDbType.Int) { Value = fLegalTypeId },
                        new SqlParameter("@f_legal_vehicle", SqlDbType.Int) { Value = fLegalVehicleId },
                        new SqlParameter("@f_companyType", SqlDbType.Int) { Value = fCompanyTypeId },
                        new SqlParameter("@f_tinNumber", SqlDbType.NVarChar) { Value = tinNumber },
                        new SqlParameter("@comment", SqlDbType.NVarChar) { Value = comment },
                        new SqlParameter("@commentTitle", SqlDbType.NVarChar) { Value = commentTitle },
                    });

                    foreach (SqlParameter parameter in command.Parameters)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }

                    command.Connection = connection;

                    try
                    {
                        command.Connection.Open();
                        command.ExecuteScalar();
                    }
                    catch (SqlException sx)
                    {
                        Console.WriteLine(sx.Message);
                    }
                }
            }
        }

        public void CreateFund(
                                string initialDate,
                                string endDate,
                                string fundName,
                                string cssfCode,
                                int fStatusId,
                                int fLegalFormId,
                                int fLegalTypeId,
                                int fLegalVehicleId,
                                string faCode,
                                string depCode,
                                string taCode,
                                int fCompanyTypeId,
                                string tinNumber,
                                string leiCode,
                                string regNumber)
        {
            string query = "EXEC sp_new_fund " +
                "@f_initialDate, @f_endDate, @f_status, " +
                "@f_registrationNumber, @f_officialFundName, " +
                "@f_leiCode, @f_cssfCode, @f_faCode, @f_depCode, @f_taCode, " +
                "@f_legalForm, @f_legalType, @f_legal_vehicle, @f_companyType, @f_tinNumber";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = this.configuration.GetConnectionString("Pharus_vFinaleConnection");
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@f_initialDate", SqlDbType.NVarChar) { Value = initialDate},
                        new SqlParameter("@f_endDate", SqlDbType.NVarChar) { Value = endDate},
                        new SqlParameter("@f_status", SqlDbType.Int) { Value = fStatusId },
                        new SqlParameter("@f_registrationNumber", SqlDbType.NVarChar) { Value = regNumber },
                        new SqlParameter("@f_officialFundName", SqlDbType.NVarChar) { Value = fundName },
                        new SqlParameter("@f_leiCode", SqlDbType.NVarChar) { Value = leiCode },
                        new SqlParameter("@f_cssfCode", SqlDbType.NVarChar) { Value = cssfCode },
                        new SqlParameter("@f_faCode", SqlDbType.NVarChar) { Value = faCode },
                        new SqlParameter("@f_depCode", SqlDbType.NVarChar) { Value = depCode },
                        new SqlParameter("@f_taCode", SqlDbType.NVarChar) { Value = taCode },
                        new SqlParameter("@f_legalForm", SqlDbType.Int) { Value = fLegalFormId },
                        new SqlParameter("@f_legalType", SqlDbType.Int) { Value = fLegalTypeId },
                        new SqlParameter("@f_legal_vehicle", SqlDbType.Int) { Value = fLegalVehicleId },
                        new SqlParameter("@f_companyType", SqlDbType.Int) { Value = fCompanyTypeId },
                        new SqlParameter("@f_tinNumber", SqlDbType.NVarChar) { Value = tinNumber },
                    });

                    foreach (SqlParameter parameter in command.Parameters)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }

                    command.Connection = connection;

                    try
                    {
                        command.Connection.Open();
                        command.ExecuteScalar();
                    }
                    catch (SqlException sx)
                    {
                        Console.WriteLine(sx.Message);
                    }
                }
            }
        }
    }
}