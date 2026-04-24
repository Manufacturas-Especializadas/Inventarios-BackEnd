using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ShippingService : IShippingService
    {
        private readonly ApplicationDbContext _context;

        public ShippingService(ApplicationDbContext context)
        {
            _context = context;            
        }

        public async Task<ShippingReleaseDto> CreateReleaseAsync(CreateShippingReleaseDto dto)
        {
            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            var release = new ShippingRelease
            {
                ShopOrder = dto.ShopOrder,
                PartNumber = dto.PartNumber,
                TargetQuantity = dto.TargetQuantity,
                PackerName = dto.PackerName,
                Status = 1,
                CreatedAt = nowInMexico,
            };

            _context.ShippingReleases.Add(release);
            await _context.SaveChangesAsync();

            return await GetActiveReleaseAsync(release.Id);
        }

        public async Task<bool> RegisterScanAsync(RegisterScanDto dto)
        {
            var release = await _context.ShippingReleases
                        .Include(r => r.Scans)
                        .FirstOrDefaultAsync(r => r.Id == dto.ShippingReleaseId);

            if(release == null || release.Status == 2)
            {
                return false;
            }

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            var scan = new ShippingScan
            {
                ShippingReleaseId = release.Id,
                ScannedLabelId = dto.ScannedLabelId,
                ScannedAt = nowInMexico,
            };

            _context.ShippingScans.Add(scan);

            if(release.Scans.Count >= release.TargetQuantity)
            {
                release.Status = 2;
                release.CompletedAt = nowInMexico;
            }

            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<ShippingReleaseDto?> GetActiveReleaseAsync(int id)
        {
            var release = await _context.ShippingReleases
                    .Include(r => r.Scans)
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (release == null) return null;

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            return new ShippingReleaseDto
            {
                Id = release.Id,
                ShopOrder = release.ShopOrder,
                PartNumber = release.PartNumber,
                TargetQuantity = release.TargetQuantity,
                CurrentScans = release.Scans.Count,
                PackerName = release.PackerName,
                Status = release.Status,
                CreatedAt = nowInMexico
            };
        }
    }
}