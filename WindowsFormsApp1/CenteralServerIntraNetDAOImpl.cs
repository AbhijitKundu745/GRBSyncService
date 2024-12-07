using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSL.GRB.WMS.Service;
using System.Net;

namespace PSL.GRB.SyncApp
{
    public class CenteralServerIntraNetDAOImpl
    {
        #region Constants

        //InsertDataQuery
        private string INSERT_WarehouseDetails = "Insert into WarehouseDetails (UniqueID,CreatedBy,ID,PlantNumber,Name,Name1,Name2,Address,City,Pin,CountryCode,RegionCode,ContactEmail,ContactPhone,CreatedAt,UpdatedAt,ServerDatetime) values(@UniqueID,@CreatedBy,@ID,@PlantNumber,@Name,@Name1,@Name2,@Address,@City,@Pin,@CountryCode,@RegionCode,@ContactEmail,@ContactPhone,@CreatedAt,@UpdatedAt,@ServerDatetime)";

        private string INSERT_BayTypeDetails = "INSERT INTO BayTypeDetails(UniqueID,CreatedBy,ID,Name,Description,CreatedAt,UpdatedAt,ServerDatetime) VALUES(@UniqueID,@CreatedBy,@ID,@Name,@Description,@CreatedAt,@UpdatedAt,@ServerDatetime)";

        private string INSERT_BayDetails = "INSERT INTO BayDetails(UniqueID,CreatedBy,ID,WarehouseID,BayName,BayTypeID,CreatedAt,UpdatedAt,ServerDatetime) VALUES(@UniqueID,@CreatedBy,@ID,@WarehouseID,@BayName,@BayTypeID,@CreatedAt,@UpdatedAt,@ServerDatetime)";

        private string INSERT_BinDetails = "INSERT INTO BinDetails(UniqueID,CreatedBy,ID,WarehouseID,BayID,BinName,CreatedAt,UpdatedAt,ServerDatetime) VALUES(@UniqueID,@CreatedBy,@ID,@WarehouseID,@BayID,@BinName,@CreatedAt,@UpdatedAt,@ServerDatetime)";

        private string INSERT_PalletDetails = "INSERT INTO PalletDetails(UniqueID,CreatedBy,ID,WarehouseID,Name,Length,Width,Height,MaxWeight,SensorID,CreatedAt,UpdatedAt,ServerDatetime) VALUES(@UniqueID,@CreatedBy,@ID,@WarehouseID,@Name,@Length,@Width,@Height,@MaxWeight,@SensorID,@CreatedAt,@UpdatedAt,@ServerDatetime)";

        private string INSERT_AssetMaster = "INSERT INTO AssetMaster (AssetID,AName,ADescription,ATypeID,IsActive,UID,CustomerID,TransactionDateTime)VALUES(@AssetID,@AName,@ADescription,@ATypeID,@IsActive,@UID,@CustomerID,@TransactionDateTime)";

        private string INSERT_STODetails = "INSERT INTO STO (STOID,STONo,CompanyCode,UserID,SupplyingPlantID,ReceivingPlantID,DeliveryNo,TruckNo,IsActive,IsProcessed,DeliveryDateTime,ModifiedDateTime,ServerDateTime, WarehouseID)" +
                                            "VALUES(@STOID, @STONo, @CompanyCode, @UserID, @SupplyingPlantID, @ReceivingPlantID, @DeliveryNo, @TruckNo, @IsActiveSTO, @IsProcessedSTO, @DeliveryDateTime, @ModifiedDateTimeSTO, @ServerDateTimeSTO, @WarehouseID)";

        private string INSERT_STOLineItems = "INSERT INTO STOLineItems (ID,STOID,ItemCode,ItemsDescription,OrderQty,DeliveryItem,DeliveryQty,UnloadedQty,BatchID,RecordStatus,IsActive,IsProcessed,ModifiedDateTime,ServerDateTime)" +
                                              "VALUES(@ID, @STOID_ITEMS, @ItemCode, @ItemsDescription, @OrderQty, @DeliveryItem, @DeliveryQty,@UnloadedQty, @BatchID,@RecordStatus, @IsActive, @IsProcessed, @ModifiedDateTime, @ServerDateTime)";

        private string INSERT_SODetails = "INSERT INTO SalesOrder (SOID,SONo,SalesDocumentType,SalesDocumentDateTime,OrderNo,DeliveryNo,DeliveryDateTime,UserID,BillingDocument,BillingType,PlantID,VendorCode,TransporterName,TruckNo,PANNo,IsActive,IsProcesses,ModifiedDateTime,ServerDateTime, WarehouseID)" +
                                            "VALUES(@SOID,@SONo,@SalesDocumentType,@SalesDocumentDateTime,@OrderNo,@DeliveryNo,@DeliveryDateTime,@UserID,@BillingDocument,@BillingType,@PlantID,@VendorCode,@TransporterName,@TruckNo,@PANNo,@IsActiveSO,@IsProcessedSO,@ModifiedDateTimeSO,@ServerDateTimeSO, @WarehouseID)";

        private string INSERT_SOLineItems = "INSERT INTO SalesOrderLineItems (ID,SOID,SalesDocumentItem,OrderQty,ItemCode,ItemDescription,LoadedQty,RecordStatus,IsActive,IsProcessed,ModifiedDateTime,ServerDateTime)" +
                                              "VALUES(@ID,@SOID_ITEMS,@SalesDocumentItem,@OrderQty,@ItemCode,@ItemDescription,@LoadedQty,@RecordStatus,@IsActive,@IsProcessed,@ModifiedDateTime,@ServerDateTime)";

        //CheckExistDataQuery
        private string ExistWareDataCheckQuery = " SELECT CASE WHEN EXISTS (SELECT * FROM WarehouseDetails WHERE PlantNumber='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";

        private string ExistBayTypeDataCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM BayTypeDetails WHERE Name ='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";

        private string ExistBayDataCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM BayDetails WHERE BayName ='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";

        private string ExistBinDataCheckQuery = "  SELECT CASE WHEN EXISTS (SELECT * FROM AssetMaster A inner join BinDetails B on A.AName=B.BinName  WHERE A.AName ='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";

        private string ExistPalletDataCheckQuery = "  SELECT CASE WHEN EXISTS (SELECT * FROM AssetMaster A inner join PalletDetails P on A.AName=P.Name  WHERE A.AName ='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";

        private string ExistsSTOCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM STO WHERE DeliveryNo='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";

        private string CheckLineItemCount = "SELECT COUNT(*) as STOLnCount from STOLineItems where STOID=(select STOID from STO where DeliveryNo='@DeliveryNo')";
        //private string ExistAssetMasterDataCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM AssetMaster WHERE AName ='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";
         private string ExistsSOCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM SalesOrder WHERE DeliveryNo='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";
         private string ExistsSOItemsCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM SalesOrderLineItems SOL, SalesOrder SO WHERE SO.SOID = SOL.SOID AND SO.DeliveryNo='@dispatchNo' AND SOL.SalesDocumentItem = '@orderItemNo' AND ItemCode = '@skuCode') THEN 1 ELSE 0 END AS IsDataExists";

        private string STOFROMDATE = "select top 1 CONVERT(char(10), ModifiedDateTime,126) as FromDate from STO order by ModifiedDateTime desc";

        private string SOFROMDATE = "select top 1 CONVERT(char(10), ModifiedDateTime,126) as FromDate from SalesOrder order by ModifiedDateTime desc";

        #endregion

        public CenteralServerIntraNetDAOImpl(string connectionString)
        {
            _connectionString = connectionString;
            //For Enabling the SSL comment out the below line
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

        }


        #region Public Proerties
        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        #endregion


        public List<WMSLogger> GetLoggers()
        {
            SqlDataReader reader;
            List<WMSLogger> itemList = null;
            try
            {
                using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
                {
                    if (centerlServerConnection.State == ConnectionState.Broken || centerlServerConnection.State == ConnectionState.Closed)
                        centerlServerConnection.Open();
                    using (SqlCommand sqlCommand = centerlServerConnection.CreateCommand())
                    {
                        sqlCommand.Connection = centerlServerConnection;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = "SELECT ID,IPAddress,Description,ParserID,URL from WMSLogger where IsActive=1 ";
                        using (reader = sqlCommand.ExecuteReader())
                        {
                            itemList = new List<WMSLogger>();
                            while (reader.Read())
                            {
                                //ItemID, ItemCode, ItemDesc, MaterialID, IsActive
                                WMSLogger itemDetl = new WMSLogger();
                                short itemId = 0;
                                if (reader["ID"] != DBNull.Value)
                                {
                                    short.TryParse(reader["ID"].ToString(), out itemId);
                                    itemDetl.ID = itemId;
                                }
                                if (reader["IPAddress"] != DBNull.Value)
                                    itemDetl.IPAddress = Convert.ToString(reader["IPAddress"]);

                                if (reader["Description"] != DBNull.Value)
                                    itemDetl.Description = Convert.ToString(reader["Description"]);

                                if (reader["URL"] != DBNull.Value)
                                {
                                    //GRBWMSService WMSService = new GRBWMSService();
                                    itemDetl.URL = Convert.ToString(reader["URL"]);

                                }

                                if (reader["ParserID"] != DBNull.Value)
                                    itemDetl.ParserID = Convert.ToInt16(reader["ParserID"]);

                                itemList.Add(itemDetl);
                                
                            }


                        }

                    }
                }
                return itemList;
            }
            catch (Exception ex)
            {
                //Psl.Chase.Utils.LogManager.Logger.LogError("Could not get Item Details List from Centeral Sever." + ex.ToString());
                throw new Exception("GetLoggers():--", ex);
            }


        }
        
        //public bool SaveWareHouseContent(WHContent wH)
        //{
        //    bool retValue = false;

        //    try
        //    {
        //        using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
        //        {
        //            centerlServerConnection.Open();
        //            using (SqlCommand command = centerlServerConnection.CreateCommand())
        //            {
        //                command.CommandText = INSERT_WarehouseDetails;
        //                command.CommandType = System.Data.CommandType.Text;
        //                command.Parameters.AddWithValue("@UniqueID", Guid.NewGuid());
        //                command.Parameters.AddWithValue("@CreatedBy", wH.createdBy);
        //                command.Parameters.AddWithValue("@ID", wH.id);
        //                command.Parameters.AddWithValue("@PlantNumber", wH.plantNumber);
        //                command.Parameters.AddWithValue("@Name", wH.name);
        //                command.Parameters.AddWithValue("@Name1", wH.name1);
        //                command.Parameters.AddWithValue("@Name2", wH.name2);
        //                command.Parameters.AddWithValue("@Address", wH.address);
        //                command.Parameters.AddWithValue("@City", wH.city);
        //                command.Parameters.AddWithValue("@Pin", wH.pin);
        //                command.Parameters.AddWithValue("@CountryCode", wH.countryCode);
        //                command.Parameters.AddWithValue("@RegionCode", wH.regionCode);
        //                command.Parameters.AddWithValue("@ContactEmail", wH.contactEmail);
        //                command.Parameters.AddWithValue("@ContactPhone", wH.contactPhone);
        //                command.Parameters.AddWithValue("@CreatedAt", wH.createdAt);
        //                command.Parameters.AddWithValue("@UpdatedAt", wH.updatedAt);
        //                command.Parameters.AddWithValue("@UpdatedBy", wH.updatedBy);
        //                command.Parameters.AddWithValue("@ServerDatetime",DateTime.Now);


        //                command.ExecuteNonQuery();
        //                retValue = true;
        //                // Check Error
        //            }
        //        }

        //        return retValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("SaveWareHouseContent() :--", ex);
        //        retValue = false;
        //        return retValue;
        //    }
        //    finally
        //    {

        //    }

        //}

        //public WMSLogin GetWMSLogin()
        //{
        //    WMSLogin login = new WMSLogin();
        //    try
        //    {
        //        using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
        //        {
        //            if (centerlServerConnection.State == ConnectionState.Broken || centerlServerConnection.State == ConnectionState.Closed)
        //                centerlServerConnection.Open();
        //            using (SqlCommand sqlCommand = centerlServerConnection.CreateCommand())
        //            {
        //                sqlCommand.Connection = centerlServerConnection;
        //                sqlCommand.CommandType = CommandType.Text;
        //                sqlCommand.CommandText = "SELECT ID,IPAddress,Description,ParserID,URL from WMSLogger";
        //                using (SqlDataReader reader = sqlCommand.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        if (reader["username"] != DBNull.Value)
        //                            login.username = Convert.ToString(reader["username"]);

        //                        if (reader["password"] != DBNull.Value)
        //                           login.password = Convert.ToString(reader["password"]);
        //                    }
        //                }
        //            }
        //        }
        //        return login;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception("GetWMSLogin() :--", ex);
        //    }
                             
        //}
        //public bool SaveBayTypesContent(BayTypesContent BTC)
        //{
        //    bool retValue = false;

        //    try
        //    {
        //        using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
        //        {
        //            centerlServerConnection.Open();
        //            using (SqlCommand command = centerlServerConnection.CreateCommand())
        //            {
        //                command.CommandText = INSERT_BayTypeDetails;
        //                command.CommandType = System.Data.CommandType.Text;
        //                command.Parameters.AddWithValue("@UniqueID", Guid.NewGuid());
        //                command.Parameters.AddWithValue("@CreatedBy", BTC.createdBy);
        //                command.Parameters.AddWithValue("@ID", BTC.id);
        //                command.Parameters.AddWithValue("@Name", BTC.name);
        //                command.Parameters.AddWithValue("@Description", BTC.description);
        //                command.Parameters.AddWithValue("@CreatedAt", BTC.createdAt);
        //                command.Parameters.AddWithValue("@UpdatedAt", BTC.updatedAt);
        //                command.Parameters.AddWithValue("@ServerDatetime", DateTime.Now);


        //                command.ExecuteNonQuery();
        //                retValue = true;
        //                // Check Error
        //            }
        //        }

        //        return retValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("SaveBayTypesContent() :--", ex);
        //        retValue = false;
        //        return retValue;
        //    }
        //    finally
        //    {

        //    }

        //}
        //public bool SaveBayContent(BaysContent BC)
        //{
        //    bool retValue = false;

        //    try
        //    {
        //        using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
        //        {
        //            centerlServerConnection.Open();
        //            using (SqlCommand command = centerlServerConnection.CreateCommand())
        //            {
        //                command.CommandText = INSERT_BayDetails;
        //                command.CommandType = System.Data.CommandType.Text;
        //                command.Parameters.AddWithValue("@UniqueID", Guid.NewGuid());
        //                command.Parameters.AddWithValue("@CreatedBy", BC.createdBy);
        //                command.Parameters.AddWithValue("@ID", BC.id);
        //                //command.Parameters.AddWithValue("@WarehouseID", BC.warehouseId);
        //                command.Parameters.AddWithValue("@BayName", BC.bayName);
        //                //command.Parameters.AddWithValue("@BayTypeID", BC.bayTypeId);
        //                command.Parameters.AddWithValue("@CreatedAt", BC.createdAt);
        //                command.Parameters.AddWithValue("@UpdatedAt", BC.updatedAt);
        //                command.Parameters.AddWithValue("@ServerDatetime", DateTime.Now);


        //                command.ExecuteNonQuery();
        //                retValue = true;
        //                // Check Error
        //            }
        //        }

        //        return retValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("SaveBayContent() :--", ex);
        //        retValue = false;
        //        return retValue;
        //    }
        //    finally
        //    {

        //    }

        //}
        //public bool SaveBinsContent(BinsContent Bin)
        //{
        //    bool retValue = false;

        //    try
        //    {
        //        using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
        //        {
        //            centerlServerConnection.Open();
        //            using (SqlCommand command = centerlServerConnection.CreateCommand())
        //            {
        //                command.CommandText = INSERT_BinDetails;
        //                command.CommandType = System.Data.CommandType.Text;
        //                command.Parameters.AddWithValue("@UniqueID", Guid.NewGuid());
        //                command.Parameters.AddWithValue("@CreatedBy", Bin.createdBy);
        //                command.Parameters.AddWithValue("@ID", Bin.id);
        //                //command.Parameters.AddWithValue("@WarehouseID", Bin.warehouseId);
        //                //command.Parameters.AddWithValue("@BayID", Bin.bayId);
        //                //command.Parameters.AddWithValue("@BinName", Bin.binName);
        //                command.Parameters.AddWithValue("@CreatedAt", Bin.createdAt);
        //                command.Parameters.AddWithValue("@UpdatedAt", Bin.updatedAt);
        //                command.Parameters.AddWithValue("@ServerDatetime", DateTime.Now);


        //                command.ExecuteNonQuery();
        //                retValue = true;
        //                // Check Error
        //            }
        //        }

        //        return retValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("SaveBinsContent() :--", ex);
        //        retValue = false;
        //        return retValue;
        //    }
        //    finally
        //    {

        //    }

        //}
        //public bool SavePalletsContent(PalletsContent PC)
        //{
        //    bool retValue = false;

        //    try
        //    {
        //        using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
        //        {
        //            centerlServerConnection.Open();
        //            using (SqlCommand command = centerlServerConnection.CreateCommand())
        //            {
        //                command.CommandText = INSERT_PalletDetails;
        //                command.CommandType = System.Data.CommandType.Text;
        //                command.Parameters.AddWithValue("@UniqueID", Guid.NewGuid());
        //                command.Parameters.AddWithValue("@CreatedBy", PC.createdBy);
        //                command.Parameters.AddWithValue("@ID", PC.id);
        //                command.Parameters.AddWithValue("@WarehouseID", PC.warehouseId);
        //                command.Parameters.AddWithValue("@Name", PC.name);
        //                command.Parameters.AddWithValue("@Length", PC.length);
        //                command.Parameters.AddWithValue("@Width", PC.width);
        //                command.Parameters.AddWithValue("@Height", PC.height);
        //                command.Parameters.AddWithValue("@MaxWeight", PC.maxWeight);
        //                command.Parameters.AddWithValue("@SensorID", PC.sensorId);
        //                command.Parameters.AddWithValue("@CreatedAt", PC.createdAt);
        //                command.Parameters.AddWithValue("@UpdatedAt", PC.updatedAt);
        //                command.Parameters.AddWithValue("@ServerDatetime", DateTime.Now);


        //                command.ExecuteNonQuery();
        //                retValue = true;
        //                // Check Error
        //            }
        //        }

        //        return retValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("SavePalletsContent() :--", ex);
        //        retValue = false;
        //        return retValue;
        //    }
        //    finally
        //    {

        //    }

        //}

        public bool CheckifExists(string CheckDataVar ,int parserID)
        {
            bool retValue = false;
            string Query = string.Empty;
            try
            {
               

                    if (parserID == 2)
                        {
                            Query = ExistWareDataCheckQuery.Replace("@CheckDataVar", CheckDataVar);
                        }
                        else if (parserID == 3)
                        {
                            Query = ExistBayTypeDataCheckQuery.Replace("@CheckDataVar", CheckDataVar);
                        }
                        else if (parserID == 4)
                        {
                            Query = ExistBayDataCheckQuery.Replace("@CheckDataVar", CheckDataVar);
                        }
                        else if (parserID == 5)
                        {
                            Query = ExistBinDataCheckQuery.Replace("@CheckDataVar", CheckDataVar);
                        }
                        else if (parserID == 6)
                        {
                            Query = ExistPalletDataCheckQuery.Replace("@CheckDataVar", CheckDataVar);
                        }


                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(Query, connection);

                    connection.Open();
                    bool result = Convert.ToBoolean(command.ExecuteScalar());
                    retValue = result;
                }
                 return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("CheckifExists() :--", ex);
                retValue = false;
                return retValue;
            }
            finally
            {

            }

        }

        public bool SaveAssetMaster(dynamic AssetDetails ,int parserID)
        {
            bool retValue = false;
            string UID = "71BFABC7-1A93-4115-BE91-DC820EF72A72";
            string CustomerID = "FA363895-4C41-4713-8781-11254F129D8D";

            try
            {
                using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
                {
                    centerlServerConnection.Open();
                    using (SqlCommand command = centerlServerConnection.CreateCommand())
                    {
                        command.CommandText = INSERT_AssetMaster;
                        command.CommandType = System.Data.CommandType.Text;
                        if (parserID == 6)
                        {
                            command.Parameters.AddWithValue("@AssetID", Guid.NewGuid());
                            command.Parameters.AddWithValue("@AName", AssetDetails.name);
                            command.Parameters.AddWithValue("@ADescription", AssetDetails.name);
                            command.Parameters.AddWithValue("@ATypeID", 2);
                            command.Parameters.AddWithValue("@IsActive", 1);
                            command.Parameters.AddWithValue("@UID", UID);
                            command.Parameters.AddWithValue("@CustomerID", CustomerID);
                            command.Parameters.AddWithValue("@TransactionDateTime", DateTime.Now);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AssetID", Guid.NewGuid());
                            command.Parameters.AddWithValue("@AName", AssetDetails.binName);
                            command.Parameters.AddWithValue("@ADescription", AssetDetails.binName);
                            command.Parameters.AddWithValue("@ATypeID", 3);
                            command.Parameters.AddWithValue("@IsActive", 1);
                            command.Parameters.AddWithValue("@UID", UID);
                            command.Parameters.AddWithValue("@CustomerID", CustomerID);
                            command.Parameters.AddWithValue("@TransactionDateTime", DateTime.Now);
                        }

                        command.ExecuteNonQuery();
                        retValue = true;
                        // Check Error
                    }
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("SaveAssetMaster() :--", ex);
                retValue = false;
                return retValue;
            }
            finally
            {

            }
        }

        public bool CheckifEntryExists(string Value,string Query)
        {
            bool retval = false;
            Query = Query.Replace("@CheckDataVar", Value);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);

                connection.Open();
                retval = Convert.ToBoolean(command.ExecuteScalar());
                connection.Close();
            }

            return retval;
        }

        public bool CheckAndUpdateTruckNoforSO(string dispatchNo,string currentTruckNumber)
        {
            bool retValue = false;

            string existingTruckID = string.Empty;
            string Query = "Select TruckNo from SalesOrder where SONo = '" + dispatchNo + "'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["TruckNo"] != DBNull.Value)
                            existingTruckID = Convert.ToString(reader["TruckNo"]);
                    }
                }
            }

            if(existingTruckID != currentTruckNumber)
            {
                string updateQuery = "Update SalesOrder set TruckNo='" + currentTruckNumber + "' where SONo='" + dispatchNo + "'"; 
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.ExecuteNonQuery();
                    retValue = true;
                }
            }
            return retValue;
        }

        public int GetSTOLineItemCount(string drnNumber)
        {
            //bool retval = false;
            int count = 0;
            string query = CheckLineItemCount;
            query = query.Replace("@DeliveryNo", drnNumber);
            //Query = Query.Replace("@CheckDataVar", Value);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["STOLnCount"] != DBNull.Value)
                            count = Convert.ToInt32(reader["STOLnCount"]);
                    }
                }
            }

            return count;
        }

        public string GetSTOFromDate()
        {
            //bool retval = false;
            string fromDate = string.Empty;
            string query = STOFROMDATE;
            
            //Query = Query.Replace("@CheckDataVar", Value);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["FromDate"] != DBNull.Value)
                            fromDate = Convert.ToString(reader["FromDate"]);
                    }
                }
            }

            return fromDate;
        }

        public string GetSOFromDate()
        {
            //bool retval = false;
            string fromDate = string.Empty;
            string query = SOFROMDATE;

            //Query = Query.Replace("@CheckDataVar", Value);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["FromDate"] != DBNull.Value)
                            fromDate = Convert.ToString(reader["FromDate"]);
                    }
                }
            }

            return fromDate;
        }

        public bool ClearSTOData(string drnNumber)
        {
            bool retValue = false;
            try
            {
                string delLineItems = "delete from STOLineItems where STOID = (select STOID from STO where DeliveryNo = '@DeliveryNo')";
                string delSTO = "delete from STO where DeliveryNo = '@DeliveryNo'";

                delLineItems = delLineItems.Replace("@DeliveryNo", drnNumber);
                delSTO = delSTO.Replace("@DeliveryNo", drnNumber);

                using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
                {
                    centerlServerConnection.Open();
                    using (SqlCommand command = centerlServerConnection.CreateCommand())
                    {
                        command.CommandText = delLineItems;
                        command.CommandType = System.Data.CommandType.Text;
                        command.ExecuteNonQuery();
                        command.CommandText = delSTO;
                        command.CommandType = System.Data.CommandType.Text;
                        command.ExecuteNonQuery();
                        retValue = true;
                        // Check Error
                    }
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("SaveAssetMaster() :--", ex);
                retValue = false;
                return retValue;
            }
            finally
            {

            }
        }
        public bool InsetSTODetails(ReceivingByTrucks STO)
        {
            bool retval = false;
            int InsertSTO = 0;
            string STOID = string.Empty;

            try
            {
                using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
                {
                    centerlServerConnection.Open();
                    foreach(var a in STO.content)
                    {
                        //This Loop gets the Truck Number ..
                        foreach (var b in a.receivings)
                        {
                            if(b.receivingNo == "230661205")
                            {
                                int j = 50;
                            }
                            //This Loop gets the Receiving Number DRN 
                            //Get The Current Items Count from the DB and check with the item count from API.
                            //If the Count is different then Delete the STO and the Line Item and Recreate it.
                            if(CheckifEntryExists(b.receivingNo,ExistsSTOCheckQuery))
                            {
                                int stoLineItemCount = GetSTOLineItemCount(b.receivingNo);
                                if (b.items.Count != stoLineItemCount)
                                {
                                    //Delete the STO and All Line Items...
                                    //int x = 50;
                                    ClearSTOData(b.receivingNo);
                                }
                            }
                            
                            foreach (var c in b.items)
                            {
                                
                                //The Loop Gets the STO number as c.orderNumber 

                                    using (SqlCommand command = centerlServerConnection.CreateCommand())
                                    {
                                        if (InsertSTO == 0 && !CheckifEntryExists(b.receivingNo, ExistsSTOCheckQuery))
                                        {
                                            command.CommandText = INSERT_STODetails;
                                            command.CommandType = System.Data.CommandType.Text;
                                            STOID = Guid.NewGuid().ToString();
                                            command.Parameters.AddWithValue("@STOID", STOID);
                                            command.Parameters.AddWithValue("@STONo", c.orderNumber);
                                            command.Parameters.AddWithValue("@CompanyCode", "GRB1");
                                            command.Parameters.AddWithValue("@UserID", b.createdBy);
                                            command.Parameters.AddWithValue("@SupplyingPlantID", string.Empty);
                                            command.Parameters.AddWithValue("@ReceivingPlantID", b.warehouse.plantNumber);
                                            command.Parameters.AddWithValue("@DeliveryNo", b.receivingNo);
                                            command.Parameters.AddWithValue("@TruckNo", a.truckNo);
                                            command.Parameters.AddWithValue("@IsActiveSTO", 1);
                                            command.Parameters.AddWithValue("@IsProcessedSTO", 0);
                                            command.Parameters.AddWithValue("@DeliveryDateTime", Convert.ToDateTime(b.eta));
                                            command.Parameters.AddWithValue("@ModifiedDateTimeSTO", b.updatedAt);
                                            command.Parameters.AddWithValue("@ServerDateTimeSTO", DateTime.Now);
                                            command.Parameters.AddWithValue("@WarehouseID", b.warehouse.id);
                                        InsertSTO = command.ExecuteNonQuery();
                                        }

                                        if(InsertSTO!=0 && CheckifEntryExists(b.receivingNo, ExistsSTOCheckQuery))
                                        {
                                            command.CommandText = INSERT_STOLineItems;
                                            command.CommandType = System.Data.CommandType.Text;
                                            command.Parameters.AddWithValue("@ID", Guid.NewGuid());
                                            command.Parameters.AddWithValue("@STOID_ITEMS", STOID);
                                           
                                            command.Parameters.AddWithValue("@ItemCode", c.sku.code);
                                            command.Parameters.AddWithValue("@ItemsDescription", c.sku.name);
                                            command.Parameters.AddWithValue("@OrderQty", c.receivingQty);
                                            command.Parameters.AddWithValue("@DeliveryItem", c.orderItemNumber);
                                            command.Parameters.AddWithValue("@DeliveryQty", string.Empty);
                                            command.Parameters.AddWithValue("@UnloadedQty", 0);
                                            command.Parameters.AddWithValue("@RecordStatus", c.sku.recordStatus);
                                            command.Parameters.AddWithValue("@BatchID", c.batchNumber);
                                            command.Parameters.AddWithValue("@IsActive", 1);
                                            command.Parameters.AddWithValue("@IsProcessed", 1);
                                          
                                            command.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
                                            command.Parameters.AddWithValue("@ServerDateTime", DateTime.Now);
                                            command.ExecuteNonQuery();
                                        }

                                        //command.Parameters.AddWithValue("@ContactPhone", wH.contactPhone);
                                        //command.Parameters.AddWithValue("@CreatedAt", wH.createdAt);
                                        //command.Parameters.AddWithValue("@UpdatedAt", wH.updatedAt);
                                        //command.Parameters.AddWithValue("@UpdatedBy", wH.updatedBy);
                                        //command.Parameters.AddWithValue("@ServerDatetime", DateTime.Now);


                                       
                                       
                                        retval = true;
                                        // Check Error
                                    }
                                
                            }

                            InsertSTO = 0;
                        }

                        
                    }
                }

                return retval;
            }

            
            catch (Exception ex)
            {
                throw new Exception("InsetSTODetails() :--", ex);
                retval = false;
                return retval;
            }
            finally
            {

            }
        }

        public bool InsetSODetails(Dispatch SO)
        {
            bool retval = false;
            int insertSOCheck = 0;
            string soId = string.Empty;

            try
            {
                using (SqlConnection centerlServerConnection = new SqlConnection(ConnectionString))
                {
                    centerlServerConnection.Open();
                    foreach (var a in SO.content)
                    {
                        foreach (var b in a.items)
                        {
                            
                            using (SqlCommand command = centerlServerConnection.CreateCommand())
                            {
                                if (insertSOCheck == 0 && !CheckifEntryExists(a.dispatchNo, ExistsSOCheckQuery))
                                {
                                    command.CommandText = INSERT_SODetails;
                                    command.CommandType = System.Data.CommandType.Text;
                                    soId = Guid.NewGuid().ToString();
                                    command.Parameters.AddWithValue("@SOID", soId);
                                    command.Parameters.AddWithValue("@SONo", a.dispatchNo);
                                    command.Parameters.AddWithValue("@SalesDocumentType", string.Empty);
                                    command.Parameters.AddWithValue("@SalesDocumentDateTime", DateTime.Now);
                                    command.Parameters.AddWithValue("@OrderNo", string.Empty);
                                    command.Parameters.AddWithValue("@DeliveryNo", a.dispatchNo);
                                    command.Parameters.AddWithValue("@DeliveryDateTime", a.billingDate);
                                    command.Parameters.AddWithValue("@UserID", a.createdBy);
                                    command.Parameters.AddWithValue("@BillingDocument", string.Empty);
                                    command.Parameters.AddWithValue("@BillingType", string.Empty);
                                    //command.Parameters.AddWithValue("@BillingDateTime", a.billingDate);
                                    command.Parameters.AddWithValue("@PlantID", a.warehouse.plantNumber);
                                    command.Parameters.AddWithValue("@VendorCode", a.payerCode);
                                    command.Parameters.AddWithValue("@TransporterName", a.payerName);
                                    command.Parameters.AddWithValue("@TruckNo", a.truckNumber);
                                    command.Parameters.AddWithValue("@PANNo", "");
                                    command.Parameters.AddWithValue("@IsActiveSO", 1);
                                    command.Parameters.AddWithValue("@IsProcessedSO", 0);
                                    command.Parameters.AddWithValue("@ModifiedDateTimeSO", DateTime.Now);
                                    command.Parameters.AddWithValue("@ServerDateTimeSO", DateTime.Now);
                                    command.Parameters.AddWithValue("@WarehouseID", a.warehouse.id);
                                    insertSOCheck = command.ExecuteNonQuery();
                                }
                                else
                                {
                                    //This is a Update Check Truck Number and Call Update if Required...
                                    try
                                    {
                                        CheckAndUpdateTruckNoforSO(a.dispatchNo, a.truckNumber);
                                    }
                                    catch(Exception ex)
                                    {
                                        Psl.Chase.Utils.LogManager.Logger.LogError("WMS Truck Update Function() ERROR.:" + ex.ToString());
                                        //Log.Error(Ex);
                                    }
                                    
                                }

                                if (insertSOCheck != 0 && CheckifEntryExists(a.dispatchNo, ExistsSOCheckQuery))
                                {
                                    if(!CheckifLineItemsExists(a.dispatchNo, b.orderItemNo, b.skuCode, ExistsSOItemsCheckQuery))
                                    {
                                        command.CommandText = INSERT_SOLineItems;
                                        command.CommandType = System.Data.CommandType.Text;
                                        command.Parameters.AddWithValue("@ID", Guid.NewGuid());
                                        command.Parameters.AddWithValue("@SOID_ITEMS", soId);
                                        command.Parameters.AddWithValue("@SalesDocumentItem", b.orderItemNo);
                                        command.Parameters.AddWithValue("@OrderQty", b.qty);
                                        command.Parameters.AddWithValue("@ItemCode", b.skuCode);
                                        command.Parameters.AddWithValue("@ItemDescription", b.skuName);
                                        command.Parameters.AddWithValue("@LoadedQty", 0);
                                        command.Parameters.AddWithValue("@RecordStatus", b.recordStatus);
                                        command.Parameters.AddWithValue("@IsActive", 1);
                                        command.Parameters.AddWithValue("@IsProcessed", 1);
                                        command.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
                                        command.Parameters.AddWithValue("@ServerDateTime", DateTime.Now);
                                        command.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        try
                                        {
                                            UpdateLineItemsforSO(a.dispatchNo, b.orderItemNo, b.skuCode, b.recordStatus, b.qty);
                                        }
                                        catch (Exception ex)
                                        {
                                            Psl.Chase.Utils.LogManager.Logger.LogError("WMS Items Update Function() ERROR.:" + ex.ToString());
                                        }
                                    }

                                }
                                retval = true;
                                // Check Error
                            }


                        }

                        insertSOCheck = 0;
                    }
                }

                return retval;
            }

            catch (Exception ex)
            {
                throw new Exception("InsetSODetails() :--", ex);
                retval = false;
                return retval;
            }
            finally
            {

            }
        }
        public bool CheckifLineItemsExists(string dispatchNo, string orderItemNo, string skuCode, string Query)
        {
            bool retval = false;
            Query = Query.Replace("@dispatchNo", dispatchNo);
            Query = Query.Replace("@orderItemNo", orderItemNo);
            Query = Query.Replace("@skuCode", skuCode);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);

                connection.Open();
                retval = Convert.ToBoolean(command.ExecuteScalar());
                connection.Close();
            }

            return retval;
        }
        public bool UpdateLineItemsforSO(string dispatchNo, string orderItemNo, string skuCode, string recordStatus, double qty)
        {
            bool retValue = false;

            string existingrecordStatus = string.Empty;
            double existingQty = 0.0;
            string Query = "Select SOL.RecordStatus, SOL.OrderQty from SalesOrderLineItems SOL, SalesOrder SO where SO.SOID = SOL.SOID and SO.DeliveryNo = '" + dispatchNo + "' and SOL.SalesDocumentItem = '" +orderItemNo+ "' and SOL.ItemCode = '"+skuCode+"'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["RecordStatus"] != DBNull.Value)
                            existingrecordStatus = Convert.ToString(reader["RecordStatus"]);
                        if (reader["OrderQty"] != DBNull.Value)
                            existingQty = Convert.ToDouble(reader["OrderQty"]);
                    }
                }
            }

            if (existingrecordStatus != recordStatus || existingQty != qty)
            {
                string updateQuery = "Update SOL set SOL.RecordStatus = '"+ recordStatus+"', SOL.OrderQty = '"+qty+ "' from SalesOrderLineItems SOL, SalesOrder SO where SO.SOID = SOL.SOID and SO.DeliveryNo = '" + dispatchNo + "' and SOL.SalesDocumentItem = '" + orderItemNo + "' and SOL.ItemCode = '" + skuCode + "'";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.ExecuteNonQuery();
                    retValue = true;
                }
            }
            return retValue;
        }

    }
    }
