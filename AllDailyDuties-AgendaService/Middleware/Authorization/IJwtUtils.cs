namespace AllDailyDuties_AgendaService.Middleware.Authorization
{
    public interface IJwtUtils
    {
        public Guid? ValidateToken(string token);
    }
}
