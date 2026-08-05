using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoBlazorMovil.Shared.DTOs;
using System.Collections.ObjectModel;

namespace DemoBlazorMovil.Services
{
    

    public class CartService
    {
        private readonly List<CartItemDTO> _items = new();

        public IReadOnlyList<CartItemDTO> Items => _items;

        private readonly List<CartTicketDTO> _tickets = new();

        public IReadOnlyList<CartTicketDTO> Tickets => _tickets;

        public event Action? OnChange;

        //articulos
        public bool AddItem(ArticuloDTO articulo, int cantidad)
        {
            var existing = _items.FirstOrDefault(x => x.ArticuloId == articulo.Id);

            int cantidadActualEnCarrito = existing?.Cantidad ?? 0;

            if (cantidadActualEnCarrito + cantidad > articulo.Stock)
                return false;

            if (existing != null)
            {
                existing.Cantidad += cantidad;
            }
            else
            {
                _items.Add(new CartItemDTO
                {
                    ArticuloId = articulo.Id,
                    Nombre = articulo.Nombre,
                    Precio = articulo.Precio,
                    Cantidad = cantidad,
                    ImagePath = articulo.ImagePath
                });
            }

            NotifyStateChanged();
            return true;
        }

        public void RemoveItem(int articuloId)
        {
            var item = _items.FirstOrDefault(x => x.ArticuloId == articuloId);
            if (item != null)
            {
                _items.Remove(item);
                NotifyStateChanged();
            }
        }

        public void Increase(int articuloId)
        {
            var item = _items.FirstOrDefault(x => x.ArticuloId == articuloId);
            if (item != null)
            {
                item.Cantidad++;
                NotifyStateChanged();
            }
        }

        public void Decrease(int articuloId)
        {
            var item = _items.FirstOrDefault(x => x.ArticuloId == articuloId);
            if (item != null)
            {
                item.Cantidad--;

                if (item.Cantidad <= 0)
                    _items.Remove(item);

                NotifyStateChanged();
            }
        }

        //tickets
        public bool AddTicket(CartTicketDTO ticket)
        {
            bool yaExiste = _tickets.Any(t =>
                t.ShowtimeId == ticket.ShowtimeId &&
                t.Asiento == ticket.Asiento);

            if (yaExiste)
                return false;

            _tickets.Add(ticket);
            NotifyStateChanged();
            return true;
        }

        public void RemoveTicket(int showtimeId, int asiento)
        {
            var ticket = _tickets.FirstOrDefault(t =>
                t.ShowtimeId == showtimeId &&
                t.Asiento == asiento);

            if (ticket != null)
            {
                _tickets.Remove(ticket);
                NotifyStateChanged();
            }
        }


        public decimal GetTotal()
        {
            var totalArticulos = _items.Sum(x => x.Subtotal);
            var totalTickets = _tickets.Sum(x => x.Subtotal);

            return totalArticulos + totalTickets;
        }

        public void Clear()
        {
            _items.Clear();
            _tickets.Clear();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
