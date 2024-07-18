using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "57f7dbef-2cf8-49c9-956a-3a9e16d6a0a5", null, "Admin", "ADMIN" },
                    { "8b80bb4a-bceb-4e48-b020-0b42da346212", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0017d7fe-f844-47fa-96b1-f6f3f280db0f", 0, "97b3d50a-96b1-45b7-b8af-49595ab775c8", "user@test.com", false, "System", "User", false, null, "USER@TEST.COM", "USER@TEST.COM", "AQAAAAIAAYagAAAAED5bP5RGNOYM25mpUBY8fL7qSkkoFRNgDpH0kvTKIaO+7KIZ/xcynptA0nhNI57zzg==", null, false, "6b78e4b6-0214-4604-9e04-6aa151e045cc", false, "user@test.com" },
                    { "9f86d912-6254-44e6-aa64-d4da31c8a999", 0, "e8b20d81-0176-4374-8d8f-448b052c3267", "admin@test.com", false, "System", "Admin", false, null, "ADMIN@TEST.COM", "ADMIN@TEST.COM", "AQAAAAIAAYagAAAAENMCfpeBVWPFHcNjU6HtG3Ynr9U2V7UM5QHeX9bL0DQ1snYmXRrajk9KoQnCcNsAjw==", null, false, "acd14a17-3eb7-44e4-ba70-e5befeac2d31", false, "admin@test.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8b80bb4a-bceb-4e48-b020-0b42da346212", "0017d7fe-f844-47fa-96b1-f6f3f280db0f" },
                    { "57f7dbef-2cf8-49c9-956a-3a9e16d6a0a5", "9f86d912-6254-44e6-aa64-d4da31c8a999" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8b80bb4a-bceb-4e48-b020-0b42da346212", "0017d7fe-f844-47fa-96b1-f6f3f280db0f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "57f7dbef-2cf8-49c9-956a-3a9e16d6a0a5", "9f86d912-6254-44e6-aa64-d4da31c8a999" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57f7dbef-2cf8-49c9-956a-3a9e16d6a0a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b80bb4a-bceb-4e48-b020-0b42da346212");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0017d7fe-f844-47fa-96b1-f6f3f280db0f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9f86d912-6254-44e6-aa64-d4da31c8a999");
        }
    }
}
