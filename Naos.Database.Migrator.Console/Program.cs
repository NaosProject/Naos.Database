// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// <summary>
//   The program entry point.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MigratorHarness
{
    using System;

    using CLAP;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The program entry point.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            WriteAsciiArt();
            Parser.Run<MigratorConsoleHarness>(args);
        }

        /// <summary>
        /// Write ASCII art.
        /// </summary>
        private static void WriteAsciiArt()
        {
            Console.WriteLine(@"______________________________________________________________________________");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|                                   `.,,.`                                   |");
            Console.WriteLine(@"|                                :++++++++++:                                |");
            Console.WriteLine(@"|                             `'+++++++++++++#+.                             |");
            Console.WriteLine(@"|                            '+++++++++++++++++++`                           |");
            Console.WriteLine(@"|                          .++++++++++++++++++++++,                          |");
            Console.WriteLine(@"|                         :+++++++++';::;'+++++++++;                         |");
            Console.WriteLine(@"|                        ;+++++++;.        `;++#++++'                        |");
            Console.WriteLine(@"|                       :++++++;`             :++++++;  `'                   |");
            Console.WriteLine(@"|                      ,++++++.                `'+++++::++                   |");
            Console.WriteLine(@"|                      ++++++`                   '++++++++                   |");
            Console.WriteLine(@"|                     '+++++                      '+++++++`                  |");
            Console.WriteLine(@"|                     +++++`                      `+++++++`                  |");
            Console.WriteLine(@"|                    '++++:                      `++++++++`                  |");
            Console.WriteLine(@"|                    +++++`                     :+++++++++`                  |");
            Console.WriteLine(@"|                    ++++:                      `;++++++++.                  |");
            Console.WriteLine(@"|                    ++++`                         ,'+++++.                  |");
            Console.WriteLine(@"|                     :;.                            `:+++.                  |");
            Console.WriteLine(@"|                                                       .;,                  |");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|               ```````                              ```..`.```              |");
            Console.WriteLine(@"|         `.,,,,,,,,,,,,,,,.`                    .;'''''''''''''';.`         |");
            Console.WriteLine(@"|       `,,,,,,,,,,,,,,,,,,,,,`               `;'''''''''''''''''''';`       |");
            Console.WriteLine(@"|      .,,,,,,,,,,,,,,,,,,,,,,,`              ''''''''''''''''''''''''       |");
            Console.WriteLine(@"|      `,,,,,,,,,,,,,,,,,,,,,,,`             `.'''''''''''''''''''''',`      |");
            Console.WriteLine(@"|      ,.`.,,,,,,,,,,,,,,,,,.`.,             .',.:;'''''''''''''';;,,'`      |");
            Console.WriteLine(@"|      ,,,,..```.......``...,,,,             .''';:,,,,,,,:,,,,,,:;'''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      ,,,,,,,,,,,,,,,,,,,,,,,,,             .''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|      .,,,,,,,,,,,,,,,,,,,,,,,.             `''''''''''''''''''''''''`      |");
            Console.WriteLine(@"|       .,,,,,,,,,,,,,,,,,,,,,.               .;'''''''''''''''''''';.       |");
            Console.WriteLine(@"|         `.,,,,,,,,,,,,,,,.`                   `,:'''''''''''''':,`         |");
            Console.WriteLine(@"|                                                      ``````                |");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|                   ````````````````````````````````                         |");
            Console.WriteLine(@"|          `````````````````````````````````````````````````````             |");
            Console.WriteLine(@"|      ````````````````````.....................```````````````````          |");
            Console.WriteLine(@"|     `````````````````.............,,,..............`````````````````       |");
            Console.WriteLine(@"|      ```````````````````.......................````````````````````        |");
            Console.WriteLine(@"|         ```````````````````````````````````````````````````````            |");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|----------------------------------------------------------------------------|");
            Console.WriteLine(@"|                                                                            |");
            Console.WriteLine(@"|         _____  ____    __  __ _                 _                          |");
            Console.WriteLine(@"|        |  __ \|  _ \  |  \/  (_)               | |                         |");
            Console.WriteLine(@"|        | |  | | |_) | | \  / |_  __ _ _ __ __ _| |_ ___  _ __              |");
            Console.WriteLine(@"|        | |  | |  _ <  | |\/| | |/ _` | '__/ _` | __/ _ \| '__|             |");
            Console.WriteLine(@"|        | |__| | |_) | | |  | | | (_| | | | (_| | || (_) | |                |");
            Console.WriteLine(@"|        |_____/|____/  |_|  |_|_|\__, |_|  \__,_|\__\___/|_|                |");
            Console.WriteLine(@"|                                  __/ |                                     |");
            Console.WriteLine(@"|                                 |___/                                      |");
            Console.WriteLine(@"|____________________________________________________________________________|");
            Console.WriteLine();
        }
    }
}
