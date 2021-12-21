namespace Com.Hd.Core.View.Control.Interface
{
    /// <summary>
    /// Control Culture Information
    /// </summary>

    public interface IHCulture : IHBaseControl
    {
        /// <summary>
        /// Provide multi language by caption resource ID. 
        /// </summary>
        string TextResourceId { get; set; }
    }
}
