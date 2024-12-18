using System;
using System.ComponentModel.DataAnnotations; // Para validações

public class ToDo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    public string Title { get; set; }

    [MaxLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // O UserId pode ser associado ao usuário autenticado
    public int UserId { get; set; }
}