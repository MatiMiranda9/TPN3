using DemoBlazorMovil.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoBlazorMovil.Services
{
    public class UserService
    {
        private readonly List<User> _users = new()
        {
            new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@demo.com",
                Password = "1234",
                ImagePath = "images/users/admin.jpg",
                IsAdmin = true
            },
            new User
            {
                Id = 2,
                Name = "Mati",
                Email = "mati@demo.com",
                Password = "1234",
                ImagePath = "images/users/user.jpg",
                IsAdmin = false
            }
        };

        private int NextId => _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;

        // 📌 CRUD Usuarios
        public IReadOnlyList<User> GetAll() => _users.OrderBy(u => u.Id).ToList();
        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
        public User Add(User user)
        {
            user.Id = NextId;
            _users.Add(user);
            return user;
        }

        public bool Update(User user)
        {
            var idx = _users.FindIndex(u => u.Id == user.Id);
            if (idx == -1) return false;
            _users[idx] = user;
            return true;
        }

        public bool Delete(int id)
        {
            var removed = _users.RemoveAll(u => u.Id == id);
            return removed > 0;
        }

        // 📌 Login con validación
        public User? ValidateLogin(string email, string password) =>
            _users.FirstOrDefault(u =>
                u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)
                && u.Password == password);

        // 📌 Helper para saber si es admin
        public bool IsAdmin(User user) => user != null && user.IsAdmin;
    }
}
