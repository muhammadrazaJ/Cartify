package com.cartify.cartify.service;

import com.cartify.cartify.dto.CategoryDto;
import com.cartify.cartify.entity.Category;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

/**
 * Service interface for category management (admin only).
 */
public interface CategoryService {

    /** Returns paginated categories (all, including inactive). */
    Page<Category> findAll(Pageable pageable);

    /** Returns paginated active categories only. */
    Page<Category> findActive(Pageable pageable);

    /** Returns category by id or throws IllegalArgumentException if not found. */
    Category getById(Long id);

    /** Creates a new category from DTO. */
    Category create(CategoryDto dto);

    /** Updates existing category identified by id from DTO. */
    Category update(Long id, CategoryDto dto);

    /** Toggles isActive flag (activate/deactivate). */
    void toggleActive(Long id);
}