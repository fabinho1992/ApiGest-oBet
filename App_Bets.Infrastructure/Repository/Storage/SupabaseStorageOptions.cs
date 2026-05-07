using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Infrastructure.Repository.Storage
{
    public class SupabaseStorageOptions
    {
        public string Url { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public string ServiceRoleKey { get; set; } = string.Empty;
    }
}
