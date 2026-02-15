package com.cartify.cartify.entity;

/**
 * User role in the Cartify system.
 * Provides Spring Security authority name (ROLE_*) for role-based access.
 */
public enum Role {
    ADMIN,
    CUSTOMER;

    /**
     * Returns the authority string required by Spring Security (e.g. ROLE_ADMIN).
     */
    public String getAuthority() {
        return "ROLE_" + name();
    }
}
