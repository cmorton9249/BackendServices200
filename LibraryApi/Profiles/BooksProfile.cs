using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using System;

namespace LibraryApi.Profiles
{
	public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            // Book -> GetBooksResponseItem
            CreateMap<Book, GetBooksResponseItem>();
            CreateMap<Book, GetBookDetailsResponse>();
            CreateMap<PostBookCreate, Book>()
                .ForMember(dest => dest.DateAdded, x => x.MapFrom((_) => DateTime.Now))
                .ForMember(dest => dest.IsInInventory, x => x.MapFrom(_ => true));
        }
    }
}
