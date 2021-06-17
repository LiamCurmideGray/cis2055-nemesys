using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;
namespace cis2055_NemesysProject.Data
{
    public class InitializeDb
    {

        //Create methods to intialize user and roles

        //Both should have a default reporter and investigator roles, also an admin role it good too

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("Reporter")).Wait();
                roleManager.CreateAsync(new IdentityRole("Investigator")).Wait();

            }
        }

        public static void SeedUsers(UserManager<NemesysUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var admin = new NemesysUser()
                {
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    AuthorAlias = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                IdentityResult result = userManager.CreateAsync(admin, "Admin123!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }


                var reporter = new NemesysUser()
                {
                    Email = "reporter@gmail.com",
                    NormalizedEmail = "REPORTER@GMAIL.COM",
                    UserName = "reporter@gmail.com",
                    AuthorAlias = "Reporter",
                    NormalizedUserName = "REPORTER@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                result = userManager.CreateAsync(reporter, "Reporter123!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(reporter, "Reporter").Wait();
                }

                reporter = new NemesysUser()
                {
                    Email = "reporter2@gmail.com",
                    NormalizedEmail = "REPORTER2@GMAIL.COM",
                    UserName = "reporter2@gmail.com",
                    AuthorAlias = "Reporter2",
                    NormalizedUserName = "REPORTER2@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                result = userManager.CreateAsync(reporter, "Reporter123!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(reporter, "Reporter").Wait();
                }

                var investigator = new NemesysUser()
                {
                    Email = "investigator@gmail.com",
                    NormalizedEmail = "INVESTIGATOR@GMAIL.COM",
                    UserName = "investigator@gmail.com",
                    AuthorAlias = "Investigator",
                    NormalizedUserName = "INVESTIGATOR@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                result = userManager.CreateAsync(investigator, "Investigator123!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(investigator, "Investigator").Wait();
                }

                investigator = new NemesysUser()
                {
                    Email = "investigator2@gmail.com",
                    NormalizedEmail = "INVESTIGATOR2@GMAIL.COM",
                    UserName = "investigator2@gmail.com",
                    AuthorAlias = "Investigator2",
                    NormalizedUserName = "INVESTIGATOR2@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                result = userManager.CreateAsync(investigator, "Investigator123!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(investigator, "Investigator").Wait();
                }
            }
        }

        public static void SeedData(cis2055nemesysContext context) {
        
        if(!context.StatusCategories.Any())
            {

                context.AddRange(

                    new StatusCategory()
                    {
                        StatusType = "Open"
                    },
                    new StatusCategory()
                    {
                        StatusType = "Closed"
                    },
                    new StatusCategory()
                    {
                        StatusType = "Being Investigated"
                    },
                    new StatusCategory()
                    {
                        StatusType = "No action Required"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Hazards.Any())
            {

                context.AddRange(

                    new Hazard()
                    {
                        HazardType = "Unsafe act"
                    },
                    new Hazard()
                    {
                        HazardType = "Condition"
                    },
                    new Hazard()
                    {
                        HazardType = "Equipment"
                    },
                    new Hazard()
                    {
                        HazardType = "Structure"
                    },
                    new Hazard()
                    {
                        HazardType = "Slip"
                    },
                    new Hazard()
                    {
                        HazardType = "Fire"
                    },
                    new Hazard()
                    {
                        HazardType = "Electrical"
                    }
                );
                context.SaveChanges();

            }

        }
    }
}
