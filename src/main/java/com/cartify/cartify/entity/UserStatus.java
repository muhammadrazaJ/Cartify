package com.cartify.cartify.entity;

/**
 * Status of a user account in the Cartify system.
 */
public enum UserStatus {
    ACTIVE,
    INACTIVE;

    /**
     * Whether the account is active and allowed to authenticate.
     */
    public boolean isActive() {
        return this == ACTIVE;
    }
}
