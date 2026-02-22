package com.cartify.cartify.service;

import com.cartify.cartify.dto.CategoryDto;
import com.cartify.cartify.entity.Category;
import com.cartify.cartify.repository.CategoryRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * Concrete implementation of {@link CategoryService} encapsulating business rules.
 */
@Service
@Transactional
public class CategoryServiceImpl implements CategoryService {

    private final CategoryRepository categoryRepository;

    public CategoryServiceImpl(CategoryRepository categoryRepository) {
        this.categoryRepository = categoryRepository;
    }

    /**
     * Returns all categories (active + inactive) with pagination.
     */
    @Override
    @Transactional(readOnly = true)
    public Page<Category> findAll(Pageable pageable) {
        return categoryRepository.findAll(pageable);
    }

    /**
     * Returns only active categories with pagination.
     */
    @Override
    @Transactional(readOnly = true)
    public Page<Category> findActive(Pageable pageable) {
        return categoryRepository.findByIsActiveTrue(pageable);
    }

    /**
     * Loads a category by id or throws an IllegalArgumentException when not found.
     */
    @Override
    @Transactional(readOnly = true)
    public Category getById(Long id) {
        return categoryRepository.findById(id)
                .orElseThrow(() -> new IllegalArgumentException("Category not found: id=" + id));
    }

    /**
     * Creates a new category from validated DTO.
     */
    @Override
    public Category create(CategoryDto dto) {
        Category category = new Category();
        category.setCategoryName(dto.getCategoryName().trim());
        category.setDescription(dto.getDescription() != null ? dto.getDescription().trim() : null);
        category.setIsActive(true);
        return categoryRepository.save(category);
    }

    /**
     * Updates an existing category's name and description (no hard delete).
     */
    @Override
    public Category update(Long id, CategoryDto dto) {
        Category category = getById(id);
        category.setCategoryName(dto.getCategoryName().trim());
        category.setDescription(dto.getDescription() != null ? dto.getDescription().trim() : null);
        return categoryRepository.save(category);
    }

    /**
     * Flips the isActive flag for soft activate/deactivate.
     */
    @Override
    public void toggleActive(Long id) {
        Category category = getById(id);
        category.setIsActive(Boolean.FALSE.equals(category.getIsActive()));
        categoryRepository.save(category);
    }
}