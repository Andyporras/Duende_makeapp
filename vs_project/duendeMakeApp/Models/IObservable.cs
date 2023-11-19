namespace duendeMakeApp.Models
{
    public interface IObservable
    {
        public void Notificar(DuendeappContext context, string titulo, string mensaje);
    }
}
