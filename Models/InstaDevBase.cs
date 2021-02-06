using System.Collections.Generic;
using System.IO;

namespace back_end_totoal.Models
{
    public class InstaDevBase
    {
        // Criar Database/File
        public void CreateFolderAndFile(string PATH, string PATH_FOLLOW){
            // Dividir caminho do PATH em pasta e arquivo
            string folder = PATH.Split("/")[0];
            // Caso o diretorio não exista e cria -- Database
            if(!Directory.Exists(folder)){Directory.CreateDirectory(folder);}
            // Caso o arquivo não exista ele cria -- usuarios.csv
            if(!File.Exists(PATH)){File.Create(PATH);}
            //Caso o arquivo não exista ele cria -- seguindo.csv
            if(!File.Exists(PATH_FOLLOW)){File.Create(PATH_FOLLOW);}
        }

        // Ler as linhas do CSV e retornar uma lista com cada item como um linha
        public List<string> ReadAllLinesCSV(string path){
            // Criar uma lista
            List<string> linhas = new List<string>();
            // Verificando quando a linha não for vazia ele adiciona uma nova
            using(StreamReader file = new StreamReader(path)){
                string linha;
                while((linha = file.ReadLine()) != null){
                    linhas.Add(linha);
                }
            }
            // Retornando a lista
            return linhas;
        }

        // Reescrever a linha exigida
        public void RewriteCSV(string path, List<string> linhas){
            using (StreamWriter ouput = new StreamWriter(path)){
                foreach (var item in linhas){
                    ouput.Write(item + '\n');
                }
            }
        }
    }
}