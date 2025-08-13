<template>
  <div class="d-flex">
    <aside class="bg-light border-end p-3" style="width: 250px; min-height: 100vh;">
      <img src="@/assets/images/logtipo.png" alt="Logo" class="mb-4" style="max-height: 60px;" />
      <nav class="nav flex-column">
        <router-link class="nav-link" to="/">🏠 Início</router-link>
        <router-link class="nav-link" to="/alunos">📚 Alunos</router-link>
       
      </nav>
    </aside>

    <main class="flex-grow-1 p-4">
        <router-view />
    </main>
  </div>
</template>

<script setup lang="ts">
</script>

<style scoped>
.nav-link.router-link-active {
  font-weight: bold;
  color: #0d6efd;
}
</style>