using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Glacier.Transfer;
using Amazon.Glacier.Model;

using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void About()
        {
            Console.WriteLine("Usage Glacer.exe [operation] [vault]");
            Console.WriteLine("operation: ");
            Console.WriteLine("\thelp :: give more information on the reqested operation");
            Console.WriteLine("\tlist :: show archives in a vault");
            Console.WriteLine("\tstore :: store an archive into a vault");
        }
        static void Main(string[] args)
        {
            string operation = String.Empty;
            string archiveToUpload = String.Empty;
            string vaultName = String.Empty;
            
            //Only need the the archive and vault.  Description sure would be nice though...
            if (args.Length == 0)
            {
                About();
            }
            else
            {           
                operation = args[0];
                
                switch (operation)
                {
                    case "list":
                        if(args.Length != 2){
                            Console.WriteLine("Inivalid parameters");
                            listArchiveHelp();
                        } else {
                            listArchives(args[1]);
                        }
                        break;
                    case "store":
                        storeArchive();
                    default:
                        break;
                } 
            }
            Console.WriteLine();
            Console.WriteLine("hit return to exit");
            Console.ReadLine();
        }

        private static void listArchiveHelp(){
            Console.WriteLine("Usage Glacier.exe list [vaultname]");
        }

        private static void listArchives(string vaultName)
        {
            InitiateJobRequest listArchiveJob = new InitiateJobRequest(){
                VaultName = vaultName,
                JobParameters = new JobParameters(){
                    Type = "inventory-retreival"}
            };
            Amazon.Glacier.AmazonGlacierClient client = new Amazon.Glacier.AmazonGlacierClient();
            
        }

        //pictures "f:\pictures\2002.zip" "Pictures from 2002 w/ flickr metadeta 200MB"
        private static void storeArchive()
        {
            if (args.Length >= 2)
            {
                vaultName = args[0].ToString();
                archiveToUpload = args[1].ToString();
                string description = args[2].ToString();

                if (File.Exists(archiveToUpload))
                {
                    Amazon.Glacier.Transfer.ArchiveTransferManager manager = new ArchiveTransferManager(Amazon.RegionEndpoint.USEast1);
                    UploadOptions options = new UploadOptions();
                    options.StreamTransferProgress += new EventHandler<Amazon.Runtime.StreamTransferProgressArgs>(progress);


                    string archiveId = manager.Upload(vaultName, description, archiveToUpload, options).ArchiveId;

                }
                else
                {
                    Console.WriteLine(String.Format("File {0} didn't exist... ", archiveToUpload));
                }
            }
        }

        public static void progress(object sender, Amazon.Runtime.StreamTransferProgressArgs args)
        {
            Console.Write(String.Format("\r{0}% complete {1}/{2}", args.PercentDone, args.TransferredBytes, args.TotalBytes));
        }
    }
}
