package com.cartify.cartify.repository;

import com.cartify.cartify.entity.Category;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

/**
 * JPA repository for Category entity with convenience queries for active records.
 */
@Repository
public interface CategoryRepository extends JpaRepository<Category, Long> {

    /** Returns a page of categories where isActive = true. */
    Page<Category> findByIsActiveTrue(Pageable pageable);

    /**
     * Explicit JPQL version to show custom query usage requirement.
     * Equivalent to {@link #findByIsActiveTrue(Pageable)} but demonstrates @Query.
     */
    @Query("SELECT c FROM Category c WHERE c.isActive = true")
    Page<Category> findActive(Pageable pageable);
}