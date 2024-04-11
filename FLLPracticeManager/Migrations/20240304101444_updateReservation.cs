using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLLPracticeManager.Migrations
{
    public partial class updateReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationSlots_reservationSlotId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "reservationSlotId",
                table: "Reservations",
                newName: "ReservationSlotId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_reservationSlotId",
                table: "Reservations",
                newName: "IX_Reservations_ReservationSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationSlots_ReservationSlotId",
                table: "Reservations",
                column: "ReservationSlotId",
                principalTable: "ReservationSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationSlots_ReservationSlotId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "ReservationSlotId",
                table: "Reservations",
                newName: "reservationSlotId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ReservationSlotId",
                table: "Reservations",
                newName: "IX_Reservations_reservationSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationSlots_reservationSlotId",
                table: "Reservations",
                column: "reservationSlotId",
                principalTable: "ReservationSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
