﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QLD.Library
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity>

            GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }



        // Use a sensible display name for views:

        [Display(Name = "Postal Code")]

        public string PostalCode { get; set; }



        // Concatenate the address info for display in tables and such:

        public string DisplayAddress
        {

            get
            {

                string dspAddress =

                    string.IsNullOrWhiteSpace(this.Address) ? "" : this.Address;

                string dspCity =

                    string.IsNullOrWhiteSpace(this.City) ? "" : this.City;

                string dspState =

                    string.IsNullOrWhiteSpace(this.State) ? "" : this.State;

                string dspPostalCode =

                    string.IsNullOrWhiteSpace(this.PostalCode) ? "" : this.PostalCode;



                return string

                    .Format("{0} {1} {2} {3}", dspAddress, dspCity, dspState, dspPostalCode);

            }

        }

    }
}