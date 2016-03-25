#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Outlook;

#endregion

namespace OutlookService
{
    public static class OutlookDataProvider
    {
        public static IEnumerable<Attachment> ExtractSalesOrderPdfs(string folderToLookIn, string storageDirectory)
        {
            var application = new Application();

            application.Session.Logon();

            if (application.Session == null)
            {
                return null;
            }

            MAPIFolder orderFolder = null;

            foreach (MAPIFolder folder in application.Session.Folders)
            {
                orderFolder = GetFolder(folder, folderToLookIn);

                if (orderFolder != null)
                {
                    break;
                }
            }
            
            var results = new List<Attachment>();

            foreach (var obj in orderFolder.Items)
            {
                if (obj is MailItem)
                {
                    var mail = (MailItem)obj;
                    if (mail.Attachments.Count > 0)
                    {
                        for (var i = 1; i <= mail.Attachments.Count; i++)
                        {
                            var isPdf = mail.Attachments[i].FileName.EndsWith(".pdf");

                            if (isPdf)
                            {
                                var fileName = Path.Combine(storageDirectory, mail.Attachments[i].FileName);
                                
                                mail.Attachments[i].SaveAsFile(fileName);

                                results.Add(new Attachment {FileName=fileName, MailId=mail.EntryID});
                            }
                        }
                    }
                }
            }

            return results;
        }

        private static MAPIFolder GetFolder(MAPIFolder rootFolder, string folderName)
        {
            if (rootFolder.Folders.Count == 0)
            {
                if (rootFolder.Name.ToLower().EndsWith("sales orders - unlaunched"))
                {
                    return rootFolder;
                }
            }
            else
            {
                foreach (MAPIFolder subFolder in rootFolder.Folders)
                {
                    var result = GetFolder(subFolder, folderName);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public static void MoveMailItem(string id, string newFolder)
        {
            throw new NotImplementedException();
        }
    }
}