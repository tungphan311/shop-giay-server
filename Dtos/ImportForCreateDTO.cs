using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ImportForCreateDTO
    {
        public int ProviderId { get; set; }

        public List<ImportDetailForCreateDTO> Details { get; set; }
    }
}