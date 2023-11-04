using Backend_Project.Domain.Entities;
using System.Linq.Expressions;
using Backend_Project.Domain.Exceptions.EntityExceptions;
using Backend_Project.Persistence.DataContexts;
using Backend_Project.Application.Foundations.NotificationServices;

namespace Backend_Project.Infrastructure.Services.NotificationsServices;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IDataContext _dataContext;

    public EmailTemplateService(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async ValueTask<EmailTemplate> CreateAsync(EmailTemplate emailTemplate, bool saveChanges = true, CancellationToken cancellationToken = default)
    {

        if (!ValidationToNull(emailTemplate))
            throw new EntityValidationException<EmailTemplate>("This a member of these emailTemplate null");

        if (ValidationExits(emailTemplate))
            throw new DuplicateEntityException<EmailTemplate>("This emailTemplate already exists");

        await _dataContext.EmailTemplates.AddAsync(emailTemplate, cancellationToken);

        if (saveChanges) await _dataContext.SaveChangesAsync();
        
        return emailTemplate;
    }
    
    public ValueTask<ICollection<EmailTemplate>> GetAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var emailTemplates = GetUndeletedEmailTemplate().Where(emailTemplate => ids.Contains(emailTemplate.Id));
        
        return new ValueTask<ICollection<EmailTemplate>>(emailTemplates.ToList());
    }
    
    public ValueTask<EmailTemplate> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var emailTemplate = GetUndeletedEmailTemplate().FirstOrDefault(emailTemplate => emailTemplate.Id == id);

        return emailTemplate is null
            ? throw new EntityNotFoundException<EmailTemplate>("EmailTemplate not found")
            : new ValueTask<EmailTemplate>(emailTemplate);
    }

    public IQueryable<EmailTemplate> Get(Expression<Func<EmailTemplate, bool>> predicate)
    {
        return GetUndeletedEmailTemplate().Where(predicate.Compile()).AsQueryable();
    }
    
    public async ValueTask<EmailTemplate> UpdateAsync(EmailTemplate emailTemplate, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (!ValidationToNull(emailTemplate))
            throw new EntityValidationException<EmailTemplate>("This a member of these emailTemplate null");

        var foundEmailTemplate = await GetByIdAsync(emailTemplate.Id, cancellationToken);

        foundEmailTemplate.Subject = emailTemplate.Subject;
        foundEmailTemplate.Body = emailTemplate.Body;

        await _dataContext.EmailTemplates.UpdateAsync(foundEmailTemplate, cancellationToken);

        if (saveChanges) await _dataContext.SaveChangesAsync();
        
        return foundEmailTemplate;
    }
    
    public async ValueTask<EmailTemplate> DeleteAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundEmailTemplate = await GetByIdAsync(id, cancellationToken);

        await _dataContext.EmailTemplates.RemoveAsync(foundEmailTemplate, cancellationToken);

        if (saveChanges) await _dataContext.SaveChangesAsync();
        
        return foundEmailTemplate;
    }

    public async ValueTask<EmailTemplate> DeleteAsync(EmailTemplate emailTemplate, bool saveChanges = true, 
        CancellationToken cancellationToken = default) 
        => await DeleteAsync(emailTemplate.Id, saveChanges, cancellationToken);
    
    //validation methods

    private static bool ValidationToNull(EmailTemplate emailTemplate)
    {
        if (string.IsNullOrWhiteSpace(emailTemplate.Subject) || string.IsNullOrWhiteSpace(emailTemplate.Body))
            return false;
        
        return true;
    }

    private bool ValidationExits(EmailTemplate emailTemplate)
    {
        var foundEmailTemplate = GetUndeletedEmailTemplate()
            .FirstOrDefault(search => search.Equals(emailTemplate));
        
        if (foundEmailTemplate is null)
            return false;
        
        return true;
    }

    private IQueryable<EmailTemplate> GetUndeletedEmailTemplate() =>
        _dataContext.EmailTemplates.Where(emailTemplate => !emailTemplate.IsDeleted).AsQueryable();
}