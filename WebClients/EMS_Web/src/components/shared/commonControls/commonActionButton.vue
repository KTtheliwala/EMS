<template>
    <Button class="p-button-aux smallIconBtn" :disabled="disabled" @click="toggleMenu" icon="pi pi-ellipsis-v" v-show="Props.menuItems.find(item=>(!item.permission || item.permission=='') || (item.permission in Props.data && Props.data[item.permission]))" />
    <Menu ref="menuToggle" :model="Props.menuItems" :popup="true">
        <template #item="{item}">
            <li class="p-menuitem" role="none" v-if="(!item.permission || item.permission=='') || (item.permission in Props.data && Props.data[item.permission])">
                <a class="p-menuitem-link" role="menuitem" @click="item.callback(Props.data)">
                    <span class="p-menuitem-icon" :class="item.icon"></span>
                    <span class="p-menuitem-text">{{item.label}}</span>
                    <span class="p-ink" role="presentation"></span>
                </a>
            </li>
        </template>
    </Menu>
</template>
<script setup lang="ts">
    import { defineProps, defineEmits, ref, inject, watch } from "vue";
    const Props = defineProps({
        menuItems: { type: Array, required: true },
        data: { type: Object },
        disabled: { type: Boolean, default: false }
    });
    const menuToggle = ref();
    const toggleMenu = (event) => {
        menuToggle.value.toggle(event);
    };
</script>