namespace Example_Ethereum_Dapp_Backend.Controllers
{
    public class UpdateMedicineBatchCommand
    {
        public string Name { get; set; }
        public string RegistrationCode { get; set; }
        public string BatchNumber { get; set; }
        public int Quantity { get; set; }
        public int ManufacturingDate { get; set; }
        public int ExpiryDate { get; set; }
        public int DosageForm { get; set; }
    }
}