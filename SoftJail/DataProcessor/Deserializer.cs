namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsDto = JsonConvert.DeserializeObject<List<ImportDepartmentsCellsDto>>(jsonString);
            var departments = new List<Department>();
            StringBuilder sb = new StringBuilder();

            foreach (var department in departmentsDto)
            {
                var areCellsValid = true;
                if (!IsModelValid(department))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var cells = new List<Cell>();
                foreach (var cell in department.Cells)
                {
                    if (!IsModelValid(cell))
                    {
                        sb.AppendLine("Invalid Data");
                        areCellsValid = false;
                        break;
                    }
                    cells.Add(new Cell() { CellNumber = cell.CellNumber, HasWindow = cell.HasWindow });
                }
                if (!areCellsValid)
                {
                    continue;
                }
                var currentDepartment = new Department()
                {
                    Name = department.Name,
                    Cells = cells
                };

                departments.Add(currentDepartment);
                sb.AppendLine($"Imported {currentDepartment.Name} with {cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDto = JsonConvert.DeserializeObject<List<ImportPrisonersDto>>(jsonString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var prisoners = new List<Prisoner>();
            StringBuilder sb = new StringBuilder();

            foreach (var prisoner in prisonersDto)
            {
                var areMailsValid = true;
                if (!IsModelValid(prisoner))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var mails = new List<Mail>();
                foreach (var mail in prisoner.Mails)
                {
                    if (!IsModelValid(mail))
                    {
                        sb.AppendLine("Invalid Data");
                        areMailsValid = false;
                        break;
                    }
                    mails.Add(new Mail()
                    {
                        Description = mail.Description,
                        Sender = mail.Sender,
                        Address = mail.Address
                    });
                }
                if (!areMailsValid)
                {
                    continue;
                }
                var currentPrisoner = new Prisoner()
                {
                    FullName = prisoner.FullName,
                    Nickname = prisoner.Nickname,
                    Age = prisoner.Age,
                    IncarcerationDate = DateTime.ParseExact(prisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = DateTime.ParseExact(prisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail = prisoner.Bail,
                    CellId = prisoner.CellId,
                    Mails = mails
                };

                prisoners.Add(currentPrisoner);
                sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficersDto[]),
                 new XmlRootAttribute("Officers"));

            var officersDto = (ImportOfficersDto[])xmlSerializer.Deserialize(new StringReader(xmlString));
            var officers = new List<Officer>();
            StringBuilder sb = new StringBuilder();

            foreach (var officer in officersDto)
            {
                if (IsModelValid(officer))
                {
                    if (!Enum.IsDefined(typeof(Position), officer.Position))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }
                    if (!Enum.IsDefined(typeof(Weapon), officer.Weapon))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var officerPrisoners = new List<OfficerPrisoner>();

                    foreach (var prisoner in officer.Prisoners)
                    {
                        var officerPrisoner = new OfficerPrisoner() { PrisonerId = prisoner.Prisoner };
                        officerPrisoners.Add(officerPrisoner);
                    }

                    var currentOfficer = Mapper.Map<Officer>(officer);
                    currentOfficer.OfficerPrisoners = officerPrisoners;

                    officers.Add(currentOfficer);
                    sb.AppendLine($"Imported {currentOfficer.FullName} ({currentOfficer.OfficerPrisoners.Count} prisoners)");
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString();
        }

        private static bool IsModelValid(object model)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            return Validator.TryValidateObject(model, validationContext, null, true);
        }
    }
}