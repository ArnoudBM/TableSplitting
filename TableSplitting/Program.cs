namespace TableSplitting
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Context;
    using Models.OneToZeroOrOne;
    using Models.TableSplitting;
    using Models.Combined;

    public static class Program
    {
        static void Main()
        {
            AppDomainValues.SetDataDirectory();

            using (var context = new TestContext())
            {
                // Enable following line to see what gets sent to the database
                //// SetContextLogging(context);

                if (context.Database.CreateIfNotExists())
                {
                    Console.WriteLine("Database created");
                }

                ExecuteWithExceptionHandling(context, TestCrudOperationsOnOneToZeroOrOne);

                ExecuteWithExceptionHandling(context, TestCrudOperationsOnSplittedTable);

                ExecuteWithExceptionHandling(context, TestCrudOperationsOnCombined);

                //ExecuteWithExceptionHandling(context, TestCrudOperationsOnCombinedWithProjectComponent);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ExecuteWithExceptionHandling(TestContext context, Action<TestContext> testMethod)
        {
            try
            {
                testMethod(context);
            }
            catch (Exception ex)
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine($"Exception raised! [{ex.GetType()}]");

                var exception = ex;

                while (exception != null)
                {
                    Console.WriteLine(exception.Message);

                    var validationException = exception as DbEntityValidationException;

                    if (validationException != null)
                    {
                        foreach (var eve in validationException.EntityValidationErrors)
                        {
                            Console.WriteLine(eve.Entry.Entity);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine($"{ve.PropertyName}: { ve.ErrorMessage}");
                            }
                        }
                    }

                    exception = exception.InnerException;
                }
                Console.WriteLine();
                Console.ForegroundColor = foregroundColor;
            }
        }

        private static void SetContextLogging(TestContext context)
        {
            context.Database.Log = (data) =>
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(data);
                Console.ForegroundColor = foregroundColor;
            };
        }

        #region One-to-Zero-Or-One tests

        private static void TestCrudOperationsOnOneToZeroOrOne(TestContext context)
        {
            Console.WriteLine("Testing one-to-zero-or-one CRUD operations");

            DeleteAllEmployeesAndLaptops(context);

            InsertEmployeeAndLaptop(context);
            SelectEmployeeAndLaptop(context);

            UpdateEmployeeAndLaptop(context);
            SelectEmployeeAndLaptop(context);

            DeleteEmployeeAndLaptop(context);
            SelectEmployeeAndLaptop(context);

            Console.WriteLine();
        }

        private static void DeleteAllEmployeesAndLaptops(TestContext context)
        {
            Console.WriteLine("Delete all employees and laptops");

            context.Employees.RemoveRange(context.Employees);

            context.SaveChanges();
        }

        private static void DeleteEmployeeAndLaptop(TestContext context)
        {
            Console.WriteLine("Delete employee and laptop");

            var employee = context.Employees.FirstOrDefault(c => c.PersonnelNumber.Equals(42));

            if (employee == null)
            {
                Console.WriteLine("Employee to delete not found");
            }
            else
            {
                context.Employees.Remove(employee);

                context.SaveChanges();

                Console.WriteLine("Employee (and associated laptop) deleted");
            }
        }

        private static void InsertEmployeeAndLaptop(TestContext context)
        {
            Console.WriteLine("Insert laptop without employee");

            var laptop = new Laptop
            {
                Code = "DELL01",
                Type = "Dell XPS 15"
            };

            context.Laptops.Add(laptop);

            try
            {
                context.SaveChanges();
                Console.WriteLine("Laptop inserted without associated employee (which is weird)");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Cannot add laptop without associated employee (which is expected)");

                // remove the invalid entity from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(laptop);
            }

            Console.WriteLine("Insert employee");

            var employee = new Employee
            {
                PersonnelNumber = 42,
                FirstName = "John",
                LastName = "Lebowski",
            };

            context.Employees.Add(employee);

            context.SaveChanges();

            Console.WriteLine("Employee inserted");

            Console.WriteLine("Insert laptop");

            context.Laptops.Add(new Laptop
            {
                Code = "NB0042",
                Type = "Dell XPS 15",
                Employee = employee
            });

            context.SaveChanges();

            Console.WriteLine("Laptop inserted");
        }

        private static void SelectEmployeeAndLaptop(TestContext context)
        {
            Console.WriteLine("Select employee and laptop");

            var employee = context.Employees.FirstOrDefault(c => c.PersonnelNumber.Equals(42));

            if (employee == null)
            {
                Console.WriteLine("Employee not found");
            }
            else
            {
                Console.WriteLine(employee.ToString());
            }

            var laptop = context.Laptops.FirstOrDefault(e => e.Employee.PersonnelNumber.Equals(42));

            if (laptop == null)
            {
                Console.WriteLine("Laptop not found");
            }
            else
            {
                Console.WriteLine(laptop.ToString());
            }
        }

        private static void UpdateEmployeeAndLaptop(TestContext context)
        {
            Console.WriteLine("Update employee and laptop");
            var employee = context.Employees.FirstOrDefault(c => c.PersonnelNumber.Equals(42));

            if (employee == null)
            {
                Console.WriteLine("Employee to update not found");
            }
            else
            {
                employee.FirstName = "The Dude";

                context.SaveChanges();

                Console.WriteLine("Employee updated");
            }

            var laptop = context.Laptops.FirstOrDefault(e => e.Employee.PersonnelNumber.Equals(42));

            if (laptop == null)
            {
                Console.WriteLine("Laptop to update not found");
            }
            else
            {
                laptop.Code = "NBK0042";

                context.SaveChanges();

                Console.WriteLine("Laptop updated");
            }
        }

        #endregion

        #region SplittedTable tests

        private static void TestCrudOperationsOnSplittedTable(TestContext context)
        {
            Console.WriteLine("Testing table splitting CRUD Operations");
            DeleteAllCustomersAndCustomerAdresses(context);

            InsertCustomerAndCustomerAddress(context);
            SelectCustomerAndCustomerAddress(context);

            UpdateCustomerAndCustomerAddress(context);
            SelectCustomerAndCustomerAddress(context);

            DeleteCustomerAndAddress(context);
            SelectCustomerAndCustomerAddress(context);
            Console.WriteLine();
        }

        private static void DeleteAllCustomersAndCustomerAdresses(TestContext context)
        {
            Console.WriteLine("Delete all customers and adresses");
            context.Customers.RemoveRange(context.Customers);

            context.SaveChanges();
        }

        private static void DeleteCustomerAndAddress(TestContext context)
        {
            Console.WriteLine("Delete customer and address");
            var customer = context.Customers.FirstOrDefault(c => c.Name.Equals("My First Customer"));

            if (customer == null)
            {
                Console.WriteLine("Customer to delete not found");
            }
            else
            {
                context.Customers.Remove(customer);

                context.SaveChanges();

                Console.WriteLine("Customer (and associated address) deleted");
            }
        }

        private static void InsertCustomerAndCustomerAddress(TestContext context)
        {
            Console.WriteLine("Insert address without customer");

            var address = new CustomerAddress
            {
                Address = "Hyde Park 1",
                City = "London"
            };

            context.CustomerAdresses.Add(address);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Address inserted without associated customer (which is weird)");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Cannot add address without associated customer (which is expected)");

                // remove the invalid entity from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(address);
            }

            Console.WriteLine("Insert customer without address");

            var customer = new Customer
            {
                Name = "My invalid Customer",
                MaxCredit = 1
            };

            context.Customers.Add(customer);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Customer inserted without associated address (which is weird)");
            }
            catch (DbEntityValidationException)
            {
                Console.WriteLine("Cannot add customer without associated addres (which is expected)");

                // remove the invalid entity from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(customer);
            }

            Console.WriteLine("Insert customer and address");

            context.Customers.Add(new Customer
            {
                Name = "My First Customer",
                MaxCredit = 30000,
                Address = new CustomerAddress
                {
                    Address = "SW1A 1AA",
                    City = "London"
                }
            });

            context.SaveChanges();

            Console.WriteLine("Customer inserted");
        }

        private static void SelectCustomerAndCustomerAddress(TestContext context)
        {
            Console.WriteLine("Select customer and address");
            var customer = context.Customers.FirstOrDefault(c => c.Name.Equals("My First Customer"));

            if (customer == null)
            {
                Console.WriteLine("Customer not found");
            }
            else
            {
                Console.WriteLine(customer.ToString());
            }
        }

        private static void UpdateCustomerAndCustomerAddress(TestContext context)
        {
            Console.WriteLine("Update customer and address");
            var customer = context.Customers.FirstOrDefault(c => c.Name.Equals("My First Customer"));

            if (customer == null)
            {
                Console.WriteLine("Customer to update not found");
            }
            else
            {
                customer.MaxCredit = 150000;
                customer.Address.City = "London (UK)";

                context.SaveChanges();

                Console.WriteLine("Customer and address updated");
            }
        }

        #endregion

        #region Combined tests

        private static void TestCrudOperationsOnCombined(TestContext context)
        {
            Console.WriteLine("Testing combined CRUD operations");
            DeleteAllProjects(context);

            InsertProjects(context);
            SelectProjects(context);

            UpdateProjects(context);
            SelectProjects(context);

            DeleteProjects(context);
            SelectProjects(context);
            Console.WriteLine();
        }

        private static void DeleteAllProjects(TestContext context)
        {
            Console.WriteLine("Delete all projects");
            context.BusinessProjectsOptions.RemoveRange(context.BusinessProjectsOptions);
            context.BusinessProjects.RemoveRange(context.BusinessProjects);
            context.ConsumerProjects.RemoveRange(context.ConsumerProjects);
            context.Projects.RemoveRange(context.Projects);

            context.SaveChanges();
        }

        private static void DeleteProjects(TestContext context)
        {
            Console.WriteLine("Delete projects");

            var project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Business Project"));

            if (project == null)
            {
                Console.WriteLine("Business project to delete not found");
            }
            else
            {
                context.Projects.Remove(project);

                context.SaveChanges();

                Console.WriteLine("Business project deleted");
            }

            project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Consumer Project"));

            if (project == null)
            {
                Console.WriteLine("Consumer project to delete not found");
            }
            else
            {
                context.Projects.Remove(project);

                context.SaveChanges();

                Console.WriteLine("Consumer project deleted");
            }
        }

        private static void InsertProjects(TestContext context)
        {
            Console.WriteLine("Insert options without business project");

            var businessProjectOptions = new BusinessProjectOptions
            {
                HasDiscount = true,
                Contact = "Joan of Arc"
            };

            context.BusinessProjectsOptions.Add(businessProjectOptions);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Options inserted without associated business project (which is weird)");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Cannot add options without associated business project (which is expected)");

                // remove the invalid entity from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(businessProjectOptions);
            }

            Console.WriteLine("Insert business project without options");

            var businessProject = new BusinessProject();

            var project = new Project
            {
                Name = "Business Project without options",
                DateAndTimeCreated = DateTime.Now,
                DateAndTimeLastModified = DateTime.Now,
                BusinessProject = businessProject
            };

            context.Projects.Add(project);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Business project inserted without associated options (which is weird)");
            }
            catch (DbEntityValidationException)
            {
                Console.WriteLine("Cannot add business project without associated options (which is expected)");

                // remove the invalid entities from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(businessProject);
                ((IObjectContextAdapter)context).ObjectContext.Detach(project);
            }

            Console.WriteLine("Insert projects");

            context.Projects.Add(new Project
            {
                Name = "My First Business Project",
                DateAndTimeCreated = DateTime.Now,
                DateAndTimeLastModified = DateTime.Now,
                BusinessProject = new BusinessProject
                {
                    Options = new BusinessProjectOptions
                    {
                        HasDiscount = true,
                        Contact = "Joan of Arc"
                    }
                }
            });

            context.SaveChanges();

            Console.WriteLine("Business project inserted");

            context.Projects.Add(new Project
            {
                Name = "My First Consumer Project",
                DateAndTimeCreated = DateTime.Now,
                DateAndTimeLastModified = DateTime.Now,
                ConsumerProject = new ConsumerProject()
            });

            context.SaveChanges();

            Console.WriteLine("Consumer project inserted");
        }

        private static void SelectProjects(TestContext context)
        {
            Console.WriteLine("Select projects");

            var project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Business Project"));

            if (project == null)
            {
                Console.WriteLine("Business project not found");
            }
            else
            {
                Console.WriteLine(project.ToString());
            }

            project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Consumer Project"));

            if (project == null)
            {
                Console.WriteLine("Consumer project not found");
            }
            else
            {
                Console.WriteLine(project.ToString());
            }
        }

        private static void UpdateProjects(TestContext context)
        {
            Console.WriteLine("Update projects");

            var project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Business Project"));

            if (project == null)
            {
                Console.WriteLine("Business project to update not found");
            }
            else
            {
                project.DateAndTimeLastModified = DateTime.Now.AddDays(1).AddHours(1);
                project.BusinessProject.Options.Contact = "Somebody else";

                context.SaveChanges();

                Console.WriteLine("Business project updated");
            }

            project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Consumer Project"));

            if (project == null)
            {
                Console.WriteLine("Consumer project to update not found");
            }
            else
            {
                project.DateAndTimeLastModified = DateTime.Now.AddDays(2).AddHours(2);

                context.SaveChanges();

                Console.WriteLine("Consumer project updated");
            }
        }

        #endregion

        #region Combined with ProjectComponent tests

        private static void TestCrudOperationsOnCombinedWithProjectComponent(TestContext context)
        {
            Console.WriteLine("Testing combined with ProjectComponent CRUD operations");
            DeleteAllProjectsWithProjectComponents(context);

            InsertProjectWithProjectComponent(context);
            SelectProjectWithProjectComponent(context);

            UpdateProjectWithProjectComponent(context);
            SelectProjectWithProjectComponent(context);

            DeleteProjectWithProjectComponent(context);
            SelectProjectWithProjectComponent(context);
            Console.WriteLine();
        }

        private static void DeleteAllProjectsWithProjectComponents(TestContext context)
        {
            Console.WriteLine("Delete all projects");
            context.Projects.RemoveRange(context.Projects);

            context.SaveChanges();
        }

        private static void DeleteProjectWithProjectComponent(TestContext context)
        {
            Console.WriteLine("Delete project");

            var project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Business Project"));

            if (project == null)
            {
                Console.WriteLine("Business project to delete not found");
            }
            else
            {
                context.Projects.Remove(project);

                context.SaveChanges();

                Console.WriteLine("Business project deleted");
            }
        }

        private static void InsertProjectWithProjectComponent(TestContext context)
        {
            Console.WriteLine("Insert project component without business project");

            var projectComponent = new ProjectComponent
            {
                Name = "Stainless steel Foobar",
                ManufacturerCode = "SSFB"
            };

            context.ProjectComponents.Add(projectComponent);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Project component inserted without associated business project (which is weird)");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Cannot add project component without associated business project (which is expected)");

                // remove the invalid entity from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(projectComponent);
            }

            Console.WriteLine("Insert options without business project");

            var businessProjectOptions = new BusinessProjectOptions
            {
                HasDiscount = true,
                Contact = "Joan of Arc"
            };

            context.BusinessProjectsOptions.Add(businessProjectOptions);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Options inserted without associated business project (which is weird)");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Cannot add options without associated business project (which is expected)");

                // remove the invalid entity from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(businessProjectOptions);
            }

            Console.WriteLine("Insert business project without options");

            var businessProject = new BusinessProject();

            var project = new Project
            {
                Name = "Business Project without options",
                DateAndTimeCreated = DateTime.Now,
                DateAndTimeLastModified = DateTime.Now,
                BusinessProject = businessProject
            };

            context.Projects.Add(project);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Business project inserted without associated options (which is weird)");
            }
            catch (DbEntityValidationException)
            {
                Console.WriteLine("Cannot add business project without associated options (which is expected)");

                // remove the invalid entities from the context
                ((IObjectContextAdapter)context).ObjectContext.Detach(businessProject);
                ((IObjectContextAdapter)context).ObjectContext.Detach(project);
            }

            Console.WriteLine("Insert project");

            context.Projects.Add(new Project
            {
                Name = "My First Business Project",
                DateAndTimeCreated = DateTime.Now,
                DateAndTimeLastModified = DateTime.Now,
                BusinessProject = new BusinessProject
                {
                    Options = new BusinessProjectOptions
                    {
                        HasDiscount = true,
                        Contact = "Joan of Arc"
                    },
                    Components = new List<ProjectComponent>
                    {
                        new ProjectComponent
                        {
                            Name = "Heavy water",
                            ManufacturerCode = "2H2O"
                        },
                        new ProjectComponent
                        {
                            Name = "Hydrogen",
                            ManufacturerCode = "H"
                        }
                    }
                }
            });

            context.SaveChanges();
        }

        private static void SelectProjectWithProjectComponent(TestContext context)
        {
            Console.WriteLine("Select project");

            var project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Business Project"));

            if (project == null)
            {
                Console.WriteLine("Business project not found");
            }
            else
            {
                Console.WriteLine(project.ToString());
            }
        }

        private static void UpdateProjectWithProjectComponent(TestContext context)
        {
            Console.WriteLine("Update project");

            var project = context.Projects.FirstOrDefault(c => c.Name.Equals("My First Business Project"));

            if (project == null)
            {
                Console.WriteLine("Business project to update not found");
            }
            else
            {
                project.DateAndTimeLastModified = DateTime.Now.AddDays(1).AddHours(1);
                project.BusinessProject.Options.Contact = "Somebody else";
                project.BusinessProject.Components.Add(new ProjectComponent
                {
                    Name = "Burn cream",
                    ManufacturerCode = "306962952508"
                });

                context.SaveChanges();

                Console.WriteLine("Business project updated");
            }
        }

        #endregion
    }
}
