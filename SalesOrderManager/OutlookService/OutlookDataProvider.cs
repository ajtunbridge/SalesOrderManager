#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using Redemption;
using MAPIFolder = Microsoft.Office.Interop.Outlook.MAPIFolder;

#endregion

namespace OutlookService
{
    public static class OutlookDataProvider
    {
        public static IEnumerable<Attachment> ExtractSalesOrderPdfs(string folderToLookIn, string storageDirectory)
        {
            var application = new Application();

            try
            {
                application.Session.Logon();
            }
            catch // exception thrown if user cancels out of profile selection dialog
            {
                return new List<Attachment>();
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
                    var mail = (MailItem) obj;

                    if (mail.Attachments.Count > 0)
                    {
                        for (var i = 1; i <= mail.Attachments.Count; i++)
                        {
                            var isPdf = mail.Attachments[i].FileName.EndsWith(".pdf");

                            if (isPdf)
                            {
                                if (!Directory.Exists(storageDirectory))
                                {
                                    Directory.CreateDirectory(storageDirectory);
                                }

                                var fileName = Path.Combine(storageDirectory, mail.Attachments[i].FileName);

                                if (File.Exists(fileName))
                                {
                                    File.Delete(fileName);
                                }

                                mail.Attachments[i].SaveAsFile(fileName);

                                results.Add(new Attachment {FileName = fileName, MailId = mail.EntryID});
                            }
                        }
                    }
                }
            }

            return results;
        }

        public static bool MoveMail(string entryId, string newFolderName)
        {
            var application = new Application();

            application.Session.Logon();

            if (application.Session == null)
            {
                return false;
            }

            MAPIFolder destFldr = null;

            foreach (MAPIFolder folder in application.Session.Folders)
            {
                destFldr = GetFolder(folder, newFolderName);

                if (destFldr != null)
                {
                    break;
                }
            }

            if (destFldr == null)
            {
                return false;
            }

            try
            {
                var rdo = new RDOSession();

                rdo.MAPIOBJECT = application.Session.MAPIOBJECT;

                var rdoMail = rdo.GetMessageFromID(entryId);

                if (rdoMail == null)
                {
                    return false;
                }

                var rodFolder = rdo.GetFolderFromID(destFldr.EntryID);

                rdoMail.Move(rodFolder);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static MAPIFolder GetFolder(MAPIFolder rootFolder, string folderName)
        {
            if (rootFolder.Folders.Count == 0)
            {
                if (rootFolder.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase))
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
    }
}