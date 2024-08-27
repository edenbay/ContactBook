using ContactBook.Models.Base;
using ContactBook.Views.Components;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Models
{
    public class ContactInformationEntry<T> : ContactInformationItem
        where T : BaseEntityForeign
    {
        public BaseEntityForeign UnderlyingEntity { get; set; }

        public ContactInformationEntry(T entry)
        {
            UnderlyingEntity = entry;
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            if (typeof(T) == typeof(Address))
            {
                var address = UnderlyingEntity as Address;
                Icon = PackIconModernKind.Home;
                Content = address.Location;
                return;
            }

            if (typeof(T) == typeof(Email))
            {
                var email = UnderlyingEntity as Email;
                Icon = PackIconModernKind.Email;
                Content = email.Address;
                return;
            }

            if (typeof(T) == typeof(Number))
            {
                var number = UnderlyingEntity as Number;
                Icon = PackIconModernKind.Phone;
                Content = number.Digits;
                return;
            }
        }
    }
}
