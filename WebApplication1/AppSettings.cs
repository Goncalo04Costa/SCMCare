namespace WebApplication1
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string JwtIssuer { get; set; } // Emitente do token JWT
        public string JwtAudience { get; set; } // Audiência do token JWT
        public int JwtExpirationInMinutes { get; set; } // Tempo de expiração do token JWT em minutos

        public string ConnectionString { get; set; } // Cadeia de conexão do banco de dados
        public string DatabaseProvider { get; set; } // Provedor de banco de dados (por exemplo, "SqlServer", "MySql", "PostgreSQL", etc.)
        public string DatabaseName { get; set; } // Nome do banco de dados

        public string ThirdPartyApiKey { get; set; } // Chave de API de terceiros
        public string ThirdPartyApiUrl { get; set; } // URL da API de terceiros

        public bool EnableLogging { get; set; } // Habilitar/desabilitar o registro de logs

        // Adicione outras propriedades de configuração, conforme necessário
    }
}
