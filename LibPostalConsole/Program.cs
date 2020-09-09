using LibPostalNet;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibPostalConsole
{
    class Program
    {
        /// <summary>
        /// An example of using the LibPostalNet library
        /// </summary>
        /// <param name="args">args[0] should be the path to libpostal data files</param>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var dataPath = args[0];
                libpostal.LibpostalSetupDatadir(dataPath);
                libpostal.LibpostalSetupParserDatadir(dataPath);
                libpostal.LibpostalSetupLanguageClassifierDatadir(dataPath);
            }
            else
            {
                Console.WriteLine("Starting Parser...");
                libpostal.LibpostalSetup();
                libpostal.LibpostalSetupParser();
                libpostal.LibpostalSetupLanguageClassifier();
            }

            var query = "Av. Beira Mar 1647 - Salgueiros, 4400-382 Vila Nova de Gaia";
            var query1 = "NatWest Markets SARs, 250 Bishopsgate, London EC2M 4AA, United Kingdom";

            string[] lines = System.IO.File.ReadAllLines(@"C:\JB\Study\LibPostal\DotNet\LibPostalNet\Data\Address.txt");

            var finalOutput = "";

            foreach (string line in lines)
            {
                var response = libpostal.LibpostalParseAddress(line, new LibpostalAddressParserOptions());

                var x = response.Results;

                var parsedAddress = line + "  :  " + System.Environment.NewLine;
                foreach (var result in x)
                {
                    Console.WriteLine(result.ToString());
                    parsedAddress = parsedAddress + result.ToString();

                }

                finalOutput = System.Environment.NewLine + finalOutput + parsedAddress + System.Environment.NewLine;

                libpostal.LibpostalAddressParserResponseDestroy(response);

                

                var expansion = libpostal.LibpostalExpandAddress(line, libpostal.LibpostalGetDefaultOptions());
                foreach (var s in expansion.Expansions)
                {
                    finalOutput = finalOutput + s + System.Environment.NewLine;
                    Console.WriteLine(s);
                }

                

            }


            System.IO.File.WriteAllText(@"C:\JB\Study\LibPostal\DotNet\LibPostalNet\Output\ParsedAdresses.txt", finalOutput);

            Console.WriteLine("Done");

            // Teardown (only called once at the end of your program)
            libpostal.LibpostalTeardown();
            libpostal.LibpostalTeardownParser();
            libpostal.LibpostalTeardownLanguageClassifier();
        }
    }
}
