using ServiceClass = Modellfabrik.Components.ServiceClass;


namespace Modellfabrik.Models
{
    /// <summary>
    /// Beinhaltet das Modell für die ServiceClass
    /// </summary>
    public class ServiceClassModel
    {
        /// <summary>
        /// Objekt vom Typ ServiceClass
        /// </summary>
        public ServiceClass Logic { get; } = ServiceClass.Logic;

    }
}