using System;
using System.Collections.Generic;
using System.IO;
using back_end_totoal.Models;
using Prototipo_BackEnd.Interfaces;

namespace Prototipo_BackEnd.Models
{
    public class Publicacao : InstaDevBase , IPublicacao
    {
        public int IdPublicacao { get; set; }
        public int IdUsuario { get; set; }
        public string Imagem { get; set; }
        public string Legenda { get; set; }
        public int Likes = 0;
        public string NomeUsuario { get;set; }
        public string ImagemUsuario { get;set; }
        public string LocalizacaoUsuario { get; set; }

        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";
        private const string PATH_DENUNCIA = "Database/denuncias.csv";

        public Publicacao(){
            CreateFolderAndFile(PATH_PUBLICACOES, PATH_LIKES);
        }

        private string Prepare(Publicacao p){
            return $"{p.IdPublicacao};{p.IdUsuario};{p.Imagem};{p.Legenda};{p.Likes};{p.NomeUsuario};{p.ImagemUsuario};{p.LocalizacaoUsuario}";
        }

        public void Create(Publicacao p){
            string[] linhasP = { Prepare(p) };
            File.AppendAllLines(PATH_PUBLICACOES, linhasP);
        }

        public List<Publicacao> ReadAll(){
            List<Publicacao> publicacoes = new List<Publicacao>();
            string[] linhas = File.ReadAllLines(PATH_PUBLICACOES);

            foreach (var item in linhas){
                string[] linha = item.Split(";");

                Publicacao publicacao = new Publicacao();

                publicacao.IdPublicacao = int.Parse(linha[0]);
                publicacao.IdUsuario = int.Parse(linha[1]);
                publicacao.Imagem = linha[2];
                publicacao.Legenda = linha[3];
                publicacao.Likes = int.Parse(linha[4]);
                publicacao.NomeUsuario = linha[5];
                publicacao.ImagemUsuario = linha[6];
                publicacao.LocalizacaoUsuario = linha[7];
                publicacoes.Add(publicacao);
            }

            return publicacoes;
        }

        public List<Publicacao> ReadAll(int id){
            List<Publicacao> publicacoes = new List<Publicacao>();

            string[] linhas = File.ReadAllLines(PATH_PUBLICACOES);

            foreach (var item in linhas){
                if(int.Parse(item.Split(";")[1]) == id){
                string[] linha = item.Split(";");

                Publicacao publicacao = new Publicacao();

                publicacao.IdPublicacao = int.Parse(linha[0]);
                publicacao.IdUsuario = int.Parse(linha[1]);
                publicacao.Imagem = linha[2];
                publicacao.Legenda = linha[3];
                publicacao.Likes = int.Parse(linha[4]);
                publicacao.NomeUsuario = linha[5];
                publicacao.ImagemUsuario = linha[6];
                publicacao.LocalizacaoUsuario = linha[7];
                
                publicacoes.Add(publicacao);
                }
            }

            return publicacoes;
        }

        public void Update(Publicacao p)
        {
            List<string> linhas = ReadAllLinesCSV(PATH_PUBLICACOES);

            linhas.RemoveAll(x => x.Split(";")[0] == p.IdPublicacao.ToString());

            linhas.Add( Prepare(p) );

            RewriteCSV(PATH_PUBLICACOES, linhas);
        }

        public void Delete(int idPublicacao)
        {
            List<string> linhas = ReadAllLinesCSV(PATH_PUBLICACOES);

            linhas.RemoveAll(x => x.Split(";")[0] == IdPublicacao.ToString());

            RewriteCSV(PATH_PUBLICACOES, linhas);
        }

        public int IdGenerator(){
            string[] linhas = File.ReadAllLines(PATH_PUBLICACOES);
            int IdUsuario = 0;
            bool loop = true;
            
            foreach (var item in linhas)
            {
                do{
                    if(IdUsuario == int.Parse(item.Split(";")[0])){
                        IdUsuario++;
                    }else{loop = false;}
                }while(loop == true);
            }

            foreach (var item in linhas)
            {
                do{
                    if(IdUsuario == int.Parse(item.Split(";")[0])){
                        IdUsuario++;
                    }else{loop = false;}
                }while(loop == true);
            }

            return IdUsuario;
        }

        public string TotalPublicacoes(int id)
        {
            int totalPub = 0;
            string[] linhasP = File.ReadAllLines(PATH_PUBLICACOES);

            foreach (var item in linhasP)
            {
                if (int.Parse(item.Split(";")[1]) == id)
                {
                    totalPub = totalPub + 1;
                }
            }
            return $"{totalPub}";
        }

        public string PrepareCSVLineLike(int idPublicacao, int idUsuario){
            return $"{idPublicacao};{idUsuario}";
        }

        public void Like(int idPublicacao, int idUsuario){
            string[] like = {PrepareCSVLineLike(idPublicacao, idUsuario)};
            File.AppendAllLines(PATH_LIKES, like);
        }

        public void CancelLike(int idPublicacao, int idUsuario){
            List<string> likes = ReadAllLinesCSV(PATH_LIKES);
            string prepareCSVLineRemove = $"{idPublicacao};{idUsuario}";
            likes.RemoveAll(x => x == prepareCSVLineRemove);
            RewriteCSV(PATH_LIKES, likes); 
        }

        // // Denuncia

        // public void Denunciar(int idDenuncia,int idPublicacao, string motivoDenuncia){
        //     string[] denuncia = {PrepareCSVLineDenuncia(idDenuncia, idPublicacao, motivoDenuncia)};
        //     File.AppendAllLines(PATH_DENUNCIA, denuncia);
        // }

        // public string PrepareCSVLineDenuncia(int idDenuncia, int idPublicacao, string idUsuario){
        //     return $"{idDenuncia};{idPublicacao};{idUsuario}";
        // }

        // public int IdGeneratorDenuncia(){
        //     string[] linhas = File.ReadAllLines(PATH_DENUNCIA);
        //     int IdUsuario = 0;
        //     bool loop = true;
            
        //     foreach (var item in linhas)
        //     {
        //         do{
        //             if(IdUsuario == int.Parse(item.Split(";")[0])){
        //                 IdUsuario++;
        //             }else{loop = false;}
        //         }while(loop == true);
        //     }

        //     foreach (var item in linhas)
        //     {
        //         do{
        //             if(IdUsuario == int.Parse(item.Split(";")[0])){
        //                 IdUsuario++;
        //             }else{loop = false;}
        //         }while(loop == true);
        //     }

        //     return IdUsuario;
        // }
    }
}