namespace Server.Utilite
{
    public static class MySQLFormattedExpression
    {
        public readonly static string ConnectionString = "Server=127.0.0.1;Database=server;Uid=root;Pwd=server;SslMode=none;";
        public readonly static string CheckIfCharacterNameExists = "select 1 from characters where name=@_name limit 1";
        public readonly static string CreateCharacter = "insert into characters (id, score, login_name, name) values (@_id, @_score, @_login_name, @_name)";
        public readonly static string DeleteCharacter = "delete from characters where id=@_id";
        public readonly static string GetCharacterByLogin = "select id as Id, score as Score, login_name as LoginName, name as Name from characters where login=@_login";
        public readonly static string UpdateCharacter = "update characters set id=@_id, score=@_score, login_name=@_login_name, name=@_name where login_name=@_login_name";
        public readonly static string CheckIfAccountIsCorrect = "select distinct 1 from accounts where login=@_login and password=@_password";
        public readonly static string CreateAccount = "insert into accounts (id, login, password) values (@_id, @_login, @_password)";
        public readonly static string GetAccountByLogin = "select id as Id, login, password as Password from accounts where login=@_login";
        public readonly static string GetAccountIdsList = "select id from accounts";
        public readonly static string GetPlayerIdsList = "select id from characters";
    }
}