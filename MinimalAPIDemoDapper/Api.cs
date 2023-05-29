using System.Runtime.CompilerServices;

namespace MinimalAPIDemoDapper
{
    //Es estática para poderla referenciar en Program.cs
    public static class Api
    {
        //El this me permite llamar el método sin parámetros en el program, porque me toma la app desde
        //donde lo llamo. Es un método de extensión
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/Users", GetUsers);
            app.MapGet("/User/{id}", GetUser);
            app.MapPost("/Users", InsertUser);
            app.MapPut("/Users", UpdateUser);
            app.MapDelete("Users", DeleteUser);
        }

        private static async Task<IResult> GetUsers(IUserData data)
        {
            try
            {
                return Results.Ok(await data.GetUsers());
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetUser(int id, IUserData data)
        {
            try
            {
                var results = await data.GetUser(id);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> InsertUser(UserModel user, IUserData data)
        {
            try
            {
                await data.InsertUser(user);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateUser(UserModel user, IUserData data)
        {
            try
            {
                await data.UpdatetUser(user);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> DeleteUser(int id, IUserData data)
        {
            try
            {
                await data.DeletetUser(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}