using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class MicrochannelService
    {
        private readonly IMicrochannelRepository _repository;

        public MicrochannelService(IMicrochannelRepository repository) => _repository = repository;
        

        public async Task<MicrochannelInventory> RegisterMovementAsync(MicrochannelCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new Exception("El código no puede estar vacío.");

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            string sanitizedCode = dto.Code.Replace("'", "-").Trim().ToUpper();
            string type = dto.TypeMovement.ToUpper();

            string containerDescription = "CONTENEDOR DESCONOCIDO";

            if (sanitizedCode.StartsWith("CONT-"))
            {
                containerDescription = "CONTENEDOR MICROCHANNEL";
            }
            else if (sanitizedCode.StartsWith("CTNA-"))
            {
                containerDescription = "CONTENEDOR NARANJA";
            }
            else if (sanitizedCode.StartsWith("CTAZ-"))
            {
                containerDescription = "CONTENEDOR AZUL";
            }

                var openCycle = await _repository.GetOpenCycleAsync(sanitizedCode);

            if (type == "ENTRADA")
            {
                if (openCycle != null)
                    throw new Exception($"El contenedor {sanitizedCode} ya está registrado como ADENTRO. Debe darle salida primero.");

                var newMovement = new MicrochannelInventory
                {
                    Code = sanitizedCode,
                    EntryDate = nowInMexico,
                    Status = "EN MESA",
                    Description = containerDescription,
                    TripNumber = null,
                    PayRollNumber = dto.PayRollNumber,
                };

                return await _repository.AddMovementAsync(newMovement);
            }
            else if (type == "SALIDA")
            {
                if (openCycle == null)
                    throw new Exception($"No se puede dar salida: El contenedor {sanitizedCode} no está dentro de la planta.");

                openCycle.ExitDate = nowInMexico;
                openCycle.Status = "FUERA DE MESA";
                openCycle.Description = containerDescription;
                openCycle.TripNumber = dto.TripNumber;
                openCycle.PayRollNumber = null;

                await _repository.UpdateMovementAsync(openCycle);

                return openCycle;
            }
            else
            {
                throw new Exception("Tipo de movimiento inválido.");
            }
        }

        public async Task<List<MicrochannelInventory>> GetRecentAsync()
        {
            return await _repository.GetRecentMovementAsync();
        }
    }
}