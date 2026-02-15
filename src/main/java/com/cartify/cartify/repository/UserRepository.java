package com.cartify.cartify.repository;

import com.cartify.cartify.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

/**
 * JPA repository for User entity.
 * Used by CustomUserDetailsService to load user by email for authentication.
 */
@Repository
public interface UserRepository extends JpaRepository<User, Long> {

    /**
     * Find user by email (used as username for login).
     */
    Optional<User> findByEmail(String email);

    /**
     * Check if a user exists with the given email.
     */
    boolean existsByEmail(String email);
}
