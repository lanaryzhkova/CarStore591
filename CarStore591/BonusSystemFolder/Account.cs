using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CarStore591 .BonusSystemFolder
{
    class Account
    {
        public int Registration(string email, string password, string name, string surname, 
            string patronymic = default, string phoneNumber = default)
        {
            if (phoneNumber != null) phoneNumber = PhoneNumberConverter(phoneNumber);
            using (Course_WorkContext db = new Course_WorkContext())
            {
                if (!IsValidEmail(email))
                    throw new Exception("Email is not valid");
                if (db.Clients.Any(e => e.Email == email) || (phoneNumber != null && db.Clients.Any(e => e.PhoneNumber == phoneNumber)))
                    throw new Exception("Account is already registered");
                Client client = new Client()
                {
                    Email = email,
                    Password = HashPassword(password),
                    PhoneNumber = phoneNumber
                };
                ClientFullName fullName = new ClientFullName()
                {
                    Name = name,
                    Surname = surname,
                    Patronymic = patronymic,
                    UserCode = client.UserCode,
                    UserCodeNavigation = client
                };

                CurrentBalance balance = new CurrentBalance()
                {
                    CurrentBalance1 = 0,
                    UserCode = client.UserCode,
                    UserCodeNavigation = client
                };
                db.Clients.Add(client);
                db.ClientFullNames.Add(fullName);
                db.CurrentBalances.Add(balance);
                db.SaveChanges();
                return client.UserCode;
            }
        }

        public int Authorization(string login, string password)
        {
            using (Course_WorkContext db = new Course_WorkContext())
            {
                var user = db.Clients.Single(e => e.Email == login || e.PhoneNumber == login);
                if (user.Password == password)
                    return user.UserCode;
                throw new Exception("Wrong password");
            }
        }

        public string Account_Info(int userCode)
        {
            using(Course_WorkContext db = new Course_WorkContext())
            {
                var client = db.Clients.Include(e => e.FullNameIndexNavigation).Single(e => e.UserCode == userCode);
                string clientInfo = $"Имя : {client.FullNameIndexNavigation.Name}\nФамилия : {client.FullNameIndexNavigation.Surname}\n";
                if (client.FullNameIndexNavigation.Patronymic != null)
                    clientInfo += $"Отчество : {client.FullNameIndexNavigation.Patronymic}\n";
                else
                    clientInfo += $"Отчество : Не указано\n";
                clientInfo += $"Email : {client.Email}\n";
                if (client.PhoneNumber != null)
                    clientInfo += $"Номер телефона : {client.PhoneNumber}";
                else
                    clientInfo += $"Номер телефона : Не указан";
                return clientInfo;
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt, buffer;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(32);
            }
            byte[] dst = new byte[49];
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }


        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer;
            if (hashedPassword == null)
                return false;
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 49) || src[0] != 0)
                return false;
            byte[] dst = new byte[16];
            Buffer.BlockCopy(src, 1, dst, 0, 16);
            byte[] buffer2 = new byte[32];
            Buffer.BlockCopy(src, 17, buffer2, 0, 32);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 1000))
                buffer = bytes.GetBytes(32);
            return buffer.SequenceEqual(buffer2);
        }

        private string PhoneNumberConverter(string phoneNumber)
        {
            StringBuilder number = new StringBuilder("+");
            phoneNumber = String.Join("", phoneNumber.Split('(', ')', '+', '-', ' '));
            if (phoneNumber.Length != 11)
                throw new Exception("Phone number is incorrect");
            if (phoneNumber[0] != 7)
                number.Append("7" + phoneNumber.Substring(1));
            else
                number.Append(phoneNumber);
            return number.ToString();
        }

        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

    }
}
