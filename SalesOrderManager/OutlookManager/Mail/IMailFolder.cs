using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookManager.Mail
{
    public interface IMailFolder
    {
        string Id { get; set; }

        string Name { get; set; }
    }

    public class OutlookMailFolder : IMailFolder
    {
        
    }
}
