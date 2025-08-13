<template>
  <div class="home">
    <header>
      <h1>🎓 Bem-vindo ao Sistema de Alunos</h1>
    </header>

  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { getAllStudents } from '../services/studentService'
import type { Student } from '../types/Student'

const students = ref<Student[]>([])

onMounted(async () => {
  students.value = await getAllStudents()
})
</script>

<style scoped>
.home {
  padding: 2rem;
  max-width: 700px;
  margin: auto;
  text-align: center;
}

header h1 {
  margin-bottom: 0.5rem;
}

.student-list {
  list-style: none;
  padding: 0;
  margin-top: 1rem;
}

.student-list li {
  background-color: #f9f9f9;
  margin-bottom: 0.5rem;
  padding: 0.75rem;
  border-radius: 5px;
  text-align: left;
}
</style>