using BuildDTOsFromExcel.FileService;

using ExcelMapper;

namespace BuildDTOsFromExcel
{
    public class Engine : IEngine
    {
        private readonly IFileParser _parser;
        private readonly IExcelToDTOMapper _excelToDTOMapper;

        public Engine(IFileParser parser, IExcelToDTOMapper excelToDTOMapper)
        {
            _parser = parser;
            _excelToDTOMapper = excelToDTOMapper;
        }

        public string Run(string[] args)
        {
            return _excelToDTOMapper.Run(args.GetAssemblyName(), _parser.Parse(args.GetFiles()))
                       ? DefaultSettings.SuccessMessage
                       : DefaultSettings.ErrorMessage;
        }
    }
}