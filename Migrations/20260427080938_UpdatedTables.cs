using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon_eCommerce_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "CustomerUsers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "CustomerUsers",
                newName: "MobileNumber");

            migrationBuilder.CreateTable(
                name: "SellerPayoutAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerUserId = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Last4Digits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerPayoutAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerPayoutAccounts_SellerUsers_SellerUserId",
                        column: x => x.SellerUserId,
                        principalTable: "SellerUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerVerificationDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerUserId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SellerVerificationId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerVerificationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerVerificationDocuments_SellerUsers_SellerUserId",
                        column: x => x.SellerUserId,
                        principalTable: "SellerUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerVerificationDocuments_SellerVerificationStatus_SellerVerificationId",
                        column: x => x.SellerVerificationId,
                        principalTable: "SellerVerificationStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SellerVerificationOverviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerUserId = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    IdentityDocsApproved = table.Column<bool>(type: "bit", nullable: false),
                    BusinessAddressPostcardVerified = table.Column<bool>(type: "bit", nullable: false),
                    VideoMeetingCompleted = table.Column<bool>(type: "bit", nullable: false),
                    TaxInterviewValidated = table.Column<bool>(type: "bit", nullable: false),
                    PayoutMethodCleared = table.Column<bool>(type: "bit", nullable: false),
                    FinalApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerVerificationOverviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerPayoutAccounts_SellerUserId",
                table: "SellerPayoutAccounts",
                column: "SellerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerVerificationDocuments_SellerUserId",
                table: "SellerVerificationDocuments",
                column: "SellerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerVerificationDocuments_SellerVerificationId",
                table: "SellerVerificationDocuments",
                column: "SellerVerificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellerPayoutAccounts");

            migrationBuilder.DropTable(
                name: "SellerVerificationDocuments");

            migrationBuilder.DropTable(
                name: "SellerVerificationOverviews");

            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "CustomerUsers",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "CustomerUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
