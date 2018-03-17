westfield-wellness-finance / src / Finance.DataAccess / Repositories / InvoiceDocumentRepository.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Finance.Dto;
using Finance.EntityFramework;
using Finance.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.DataAccess.Repositories
{
    public class InvoiceDocumentRepository : IInvoiceDocumentRepository
    {
        private readonly IMapper _mapper;
        private readonly ITransactionsSqlContext _context;

        public InvoiceDocumentRepository(ITransactionsSqlContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> AddAsync(InvoiceDocumentDto dto)
        {
            // TODO: Fluent validation

            var invoice = await _context.Invoices
                .Include(inv => inv.InvoiceDocuments)
                .Where(w => w.Id == dto.InvoiceId)
                .SingleOrDefaultAsync();

            if (invoice == null)
            {
                throw new ArgumentException("Invalid Invoice ID", nameof(dto.InvoiceId));
            }

            var doc = _mapper.Map<InvoiceDocument>(dto);

            invoice.InvoiceDocuments.Add(doc);

            _context.SaveChanges();

            return doc.Id;
        }

        public async Task<InvoiceDocument> GetInvoiceDocumentAsync(int invoiceDocId)
        {
            var invoiceDocument = await _context.InvoiceDocuments.Include(id => id.Document)
                                    .Where(id => id.DocumentId == invoiceDocId)
                                    .SingleOrDefaultAsync();

            if (invoiceDocument == null)
            {
                throw new ArgumentException("Invalid Invoice ID", nameof(InvoiceDocument));
            }

            return invoiceDocument;
        }
    }
}