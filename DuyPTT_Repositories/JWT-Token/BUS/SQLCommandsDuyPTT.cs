namespace DuyPTT_Repositories.JWT_Token.BUS
{
	public class SQLCommandsDuyPTT
	{
		public string StoreGetUser = @"DUYPTT_DATABASE.dbo.sp_GetUser";
		public string StoreGetRoleUser = @"DUYPTT_DATABASE.dbo.sp_GetRoleUser";
		public string StoreGetUserName = @"DUYPTT_DATABASE.dbo.sp_GetUsername";
		public string StoreInsertUser = @"DUYPTT_DATABASE.dbo.sp_InsertUser";
	}
}
