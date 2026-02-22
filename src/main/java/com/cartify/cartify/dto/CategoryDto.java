package com.cartify.cartify.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;

/**
 * DTO for admin category form binding.
 * Carries validated fields from the Thymeleaf form to the service layer.
 */
public class CategoryDto {

    /** Category name is required and limited to 255 characters. */
    @NotBlank(message = "Category name is required")
    @Size(max = 255, message = "Category name must be at most 255 characters")
    private String categoryName;

    /** Optional description with an upper bound to keep payload small. */
    @Size(max = 1000, message = "Description must be at most 1000 characters")
    private String description;

    public String getCategoryName() { return categoryName; }
    public void setCategoryName(String categoryName) { this.categoryName = categoryName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }
}