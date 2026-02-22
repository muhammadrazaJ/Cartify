package com.cartify.cartify.controller.admin;

import com.cartify.cartify.dto.CategoryDto;
import com.cartify.cartify.entity.Category;
import com.cartify.cartify.service.CategoryService;
import jakarta.validation.Valid;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

/**
 * Admin-only controller for category management.
 * All routes are protected by SecurityConfig (/admin/** → ROLE_ADMIN).
 */
@Controller
@RequestMapping("/admin/categories")
public class AdminCategoryController {

    private final CategoryService categoryService;

    public AdminCategoryController(CategoryService categoryService) {
        this.categoryService = categoryService;
    }

    /**
     * Lists categories with pagination. Optional filter for active-only.
     */
    @GetMapping
    public String listCategories(
            @RequestParam(name = "page", defaultValue = "0") int page,
            @RequestParam(name = "size", defaultValue = "10") int size,
            @RequestParam(name = "activeOnly", defaultValue = "false") boolean activeOnly,
            Model model
    ) {
        PageRequest pageable = PageRequest.of(Math.max(page, 0), Math.max(size, 1));
        Page<Category> categories = activeOnly
                ? categoryService.findActive(pageable)
                : categoryService.findAll(pageable);

        model.addAttribute("categories", categories);
        model.addAttribute("activeOnly", activeOnly);
        return "admin/category-list";
    }

    /**
     * Displays the new category form.
     */
    @GetMapping("/new")
    public String showCreateForm(Model model) {
        model.addAttribute("categoryDto", new CategoryDto());
        model.addAttribute("formAction", "/admin/categories");
        model.addAttribute("formTitle", "Add Category");
        return "admin/category-form";
    }

    /**
     * Handles form submission to create a new category.
     */
    @PostMapping
    public String createCategory(
            @Valid @ModelAttribute("categoryDto") CategoryDto dto,
            BindingResult bindingResult,
            RedirectAttributes redirectAttributes,
            Model model
    ) {
        if (bindingResult.hasErrors()) {
            model.addAttribute("formAction", "/admin/categories");
            model.addAttribute("formTitle", "Add Category");
            return "admin/category-form";
        }

        categoryService.create(dto);
        redirectAttributes.addFlashAttribute("success", "Category created successfully");
        return "redirect:/admin/categories";
    }

    /**
     * Displays the edit form for a category.
     */
    @GetMapping("/edit/{id}")
    public String showEditForm(@PathVariable("id") Long id, Model model, RedirectAttributes redirectAttributes) {
        try {
            Category category = categoryService.getById(id);
            CategoryDto dto = new CategoryDto();
            dto.setCategoryName(category.getCategoryName());
            dto.setDescription(category.getDescription());

            model.addAttribute("categoryDto", dto);
            model.addAttribute("formAction", "/admin/categories/update/" + id);
            model.addAttribute("formTitle", "Edit Category");
            return "admin/category-form";
        } catch (IllegalArgumentException ex) {
            redirectAttributes.addFlashAttribute("error", ex.getMessage());
            return "redirect:/admin/categories";
        }
    }

    /**
     * Processes the edit form and updates the category.
     */
    @PostMapping("/update/{id}")
    public String updateCategory(
            @PathVariable("id") Long id,
            @Valid @ModelAttribute("categoryDto") CategoryDto dto,
            BindingResult bindingResult,
            RedirectAttributes redirectAttributes,
            Model model
    ) {
        if (bindingResult.hasErrors()) {
            model.addAttribute("formAction", "/admin/categories/update/" + id);
            model.addAttribute("formTitle", "Edit Category");
            return "admin/category-form";
        }

        try {
            categoryService.update(id, dto);
            redirectAttributes.addFlashAttribute("success", "Category updated successfully");
        } catch (IllegalArgumentException ex) {
            redirectAttributes.addFlashAttribute("error", ex.getMessage());
        }
        return "redirect:/admin/categories";
    }

    /**
     * Toggles active flag (soft activate/deactivate).
     */
    @PostMapping("/toggle/{id}")
    public String toggleCategory(
            @PathVariable("id") Long id,
            RedirectAttributes redirectAttributes
    ) {
        try {
            categoryService.toggleActive(id);
            redirectAttributes.addFlashAttribute("success", "Category status updated");
        } catch (IllegalArgumentException ex) {
            redirectAttributes.addFlashAttribute("error", ex.getMessage());
        }
        return "redirect:/admin/categories";
    }
}