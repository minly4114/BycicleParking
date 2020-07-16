using ICS.Core.Dtos.Income;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Web.User.Models
{
    public class MessageModel
    {
        public IncomeMessage Message { get; set; }
        public string Sessions { get; set; }
    }
}
